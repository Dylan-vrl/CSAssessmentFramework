using System.Collections;
using CSFramework.Core;
using CSFramework.Presets;
using CSFramework.Presettables;
using UnityEngine;
using UnityEngine.Rendering;

namespace CSFramework.Extensions
{
    [RequireComponent(typeof(Volume))]
    public class DepthOfField : Extension<Volume, DepthOfFieldPreset>
    {
        public override PresettableCategory GetCategory() => PresettableCategory.Vision;

        private Volume _postProcessing;

        private float _changedBlurStart;
        private float _changedBlurMax;
        private UnityEngine.Rendering.Universal.DepthOfField _depthOfField;
        private CharacterController _xrChara;

        private Vector3 _lastPos;
        private Quaternion _lastRot;
        private Vector3 _lastAngularVelocity;
        private int _changing;
        private Coroutine _changeBlurRoutine;

        private void Awake()
        {
            _postProcessing = GetComponent<Volume>();
        }

        //TODO: figure out simple way to get rid of non-dirtying
        // When disabling this mono in edit mode, the DOF will go back to the shown values in play mode
        // This can be fixed by adding #ifUnityEditor and serialized objects, with applymodifiedproperties
        private void OnEnable()
        {
            _changedBlurStart = Preset.ConstantBlur ? Preset.BlurStartDistance : 500;
            _changedBlurMax = Preset.ConstantBlur ? Preset.BlurMaxDistance : 500;
            if (_postProcessing == null)
            {
                Debug.LogWarning("No Post Processing set");
                return;
            }

            if (!_postProcessing.profile.TryGet(out _depthOfField))
            {
                Debug.LogWarning("No Depth of Field found");
                return;
            }

            _depthOfField.active = Preset.ConstantBlur || Preset.DynamicBlur;

            UpdateDof(_changedBlurStart, _changedBlurMax);
        }

        private void Start()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            _xrChara = GameHandler.Instance.XROrigin.GetComponent<CharacterController>();
        }


        private void Update()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (Preset.DynamicBlur && GameHandler.State == GameHandler.StateType.Playing)
            {
                var m = false;
                var t = false;
                var v = _xrChara.velocity.magnitude;
                Vector3 pos = _xrChara.transform.position;
                // If moving
                //velocity sometimes stick to a value even when stopped
                if (v > 0.3 && pos != _lastPos)
                {
                    m = true;
                }

                _lastPos = pos;

                Quaternion rot = GameHandler.Instance.XROrigin.transform.rotation;
                Quaternion deltaRot = rot * Quaternion.Inverse(_lastRot);
                var eulerRot = new Vector3(Mathf.DeltaAngle(0, deltaRot.eulerAngles.x),
                    Mathf.DeltaAngle(0, deltaRot.eulerAngles.y), Mathf.DeltaAngle(0, deltaRot.eulerAngles.z));
                Vector3 angularVelocity = eulerRot / Time.fixedDeltaTime;

                // If turning
                if (_lastAngularVelocity.magnitude > 10 && angularVelocity.magnitude > 10)
                {
                    t = true;
                }

                _lastAngularVelocity = angularVelocity;
                _lastRot = rot;
                if (_changing != (m || t ? 1 : -1))
                {
                    if (_changeBlurRoutine != null) StopCoroutine(_changeBlurRoutine);
                    _changeBlurRoutine = StartCoroutine(ChangeBlur(m || t, Time.time));
                }

            }

        }

        private IEnumerator ChangeBlur(bool moving, float time)
        {

            _changing = moving ? 1 : -1;
            var f = 0f;
            float startBlur;
            float maxBlur;
            while (f < 1)
            {
                f = Mathf.Clamp01(Time.time - time + 0.2f);
                if (moving)
                {
                    startBlur = Mathf.Lerp(_depthOfField.gaussianStart.value, Preset.DynamicBlurStartDistance, f);
                    maxBlur = Mathf.Lerp(_depthOfField.gaussianEnd.value, Preset.DynamicBlurMaxDistance, f);
                }
                else
                {
                    startBlur = Mathf.Lerp(_depthOfField.gaussianStart.value, _changedBlurStart, f);
                    maxBlur = Mathf.Lerp(_depthOfField.gaussianEnd.value, _changedBlurMax, f);
                }

                UpdateDof(startBlur, maxBlur);
                yield return null;
            }
        }

        private void UpdateDof(float startDist, float maxDist)
        {
            _depthOfField.gaussianStart.value = startDist;
            _depthOfField.gaussianEnd.value = maxDist;
            _depthOfField.gaussianMaxRadius.value = Preset.BlurIntensity;
        }


        private void OnValidate()
        {
            if (_postProcessing != null && _depthOfField != null)
            {
                _changedBlurStart = Preset.ConstantBlur ? Preset.BlurStartDistance : 500;
                _changedBlurMax = Preset.ConstantBlur ? Preset.BlurMaxDistance : 500;
                _depthOfField.active = Preset.ConstantBlur || Preset.DynamicBlur;
                UpdateDof(_changedBlurStart, _changedBlurMax);
            }
        }


        private void OnDisable()
        {
            if (_postProcessing != null && _depthOfField != null)
            {
                //_depthOfField.intensity.value = _vignette.intensity.min;
                _depthOfField.active = false;
            }
        }
    }
}