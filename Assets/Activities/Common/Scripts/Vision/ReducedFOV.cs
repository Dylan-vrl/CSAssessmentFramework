using System.Collections;
using CSFramework.Core;
using CSFramework.Presets;
using CSFramework.Presettables;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static GameStateManager.GameState;

namespace CSFramework.Extensions
{
    [RequireComponent(typeof(Volume))]
    public class ReducedFOV : Extension<Volume, ReducedFOVPreset>
    {
        public override PresettableCategory GetCategory() => PresettableCategory.Vision;
        
        private Volume _postProcessing;
        
        private Vignette _vignette;
        private float _changedFOVIntensity;
        private float _changedFOVSmoothness;
        private CharacterController _xrChara;

        private Vector3 _lastPos;
        private Quaternion _lastRot;
        private Vector3 _lastAngularVelocity;
        private int _changing;
        private Coroutine _changeVignetteRoutine;

        private void Awake()
        {
            _postProcessing = GetComponent<Volume>();
        }

        private void OnEnable()
        {
            _changedFOVIntensity = Preset.ConstantFOV ? Preset.Intensity : 0;
            _changedFOVSmoothness = Preset.ConstantFOV ? Preset.Smoothness : 0;
            if (_postProcessing == null)
            {
                Debug.LogWarning("No Post Processing set");
                return;
            }
            if (!_postProcessing.profile.TryGet(out _vignette))
            {
                Debug.LogWarning("No vignette found");
                return;
            }

            _vignette.active = Preset.ConstantFOV || Preset.DynamicFOV;

            UpdateFOV(_changedFOVIntensity, _changedFOVSmoothness);
        }

        private void Start()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            _xrChara = ExperimentController.Instance.XROrigin.GetComponent<CharacterController>();
        }
        
        private void Update()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (Preset.DynamicFOV && GameStateManager.State == Playing)
            {
                var m = false;
                var t = false;
                var v = _xrChara.velocity.magnitude;
                Vector3 pos = _xrChara.transform.position;
                //velocity sometimes stick to a value even when stopped
                if (v > 0.3 && pos != _lastPos)
                {
                    m = true;
                }

                _lastPos = pos;

                Quaternion rot = ExperimentController.Instance.XROrigin.transform.rotation;
                Quaternion deltaRot = rot * Quaternion.Inverse(_lastRot);
                var eulerRot =  new Vector3( Mathf.DeltaAngle( 0, deltaRot.eulerAngles.x ), Mathf.DeltaAngle( 0, deltaRot.eulerAngles.y ),Mathf.DeltaAngle( 0, deltaRot.eulerAngles.z ) );
                Vector3 angularVelocity = eulerRot / Time.fixedDeltaTime;
                
                if (_lastAngularVelocity.magnitude > 10 && angularVelocity.magnitude > 10)
                {
                    t = true;
                }

                _lastAngularVelocity = angularVelocity;
                _lastRot = rot;
                // if we are not already changing the vignette to the same destination values.
                if (_changing != (m || t ? 1 : -1))
                {
                    if(_changeVignetteRoutine != null) StopCoroutine(_changeVignetteRoutine);
                    _changeVignetteRoutine = StartCoroutine(ChangeVignette(m || t));
                }
            }
        }
        
        /// <summary>
        /// Changes the vignette values to either the dynamic or constant values.
        /// </summary>
        /// <param name="moving">If true, changes to dynamic values of vignette.
        /// If false, changes to constant values of vignette. </param>
        private IEnumerator ChangeVignette(bool moving)
        {
            var time = Time.time;
            _changing = moving ? 1 : -1;
            var f = 0f;
            float fovIntensity;
            float fovSmooth;
            // we use intensity to measure progress.
            var currentI= (_vignette.intensity.value - _changedFOVIntensity)/(Preset.DynamicIntensity - _changedFOVIntensity);
            currentI = moving ? currentI : 1 - currentI;
            while (f < 1)
            {
                f = Mathf.Clamp01((Time.time - time)/Preset.TransitionSpeed + currentI);
                if (moving)
                {
                    fovIntensity = Mathf.Lerp(_changedFOVIntensity, Preset.DynamicIntensity, f);
                    fovSmooth = Mathf.Lerp(_changedFOVSmoothness, Preset.DynamicSmoothness, f);
                }
                else
                {
                    fovIntensity = Mathf.Lerp(Preset.DynamicIntensity, _changedFOVIntensity, f);
                    fovSmooth = Mathf.Lerp(Preset.DynamicSmoothness, _changedFOVSmoothness, f);
                }
                UpdateFOV(fovIntensity, fovSmooth);
                yield return null;
            }
        }

        private void OnValidate()
        {
            if (_postProcessing != null && _vignette != null)
            {
                _changedFOVIntensity = Preset.ConstantFOV ? Preset.Intensity : 0;
                _changedFOVSmoothness = Preset.ConstantFOV ? Preset.Smoothness : 0;
                _vignette.active = Preset.ConstantFOV || Preset.DynamicFOV;
                UpdateFOV(_changedFOVIntensity, _changedFOVSmoothness);
            }
        }

        private void UpdateFOV(float fovIntensity, float fovSmoothness)
        {
            _vignette.intensity.value = fovIntensity;
            _vignette.smoothness.value = fovSmoothness;
        }

        private void OnDisable()
        {
            if (_postProcessing != null && _vignette != null)
            {
                _vignette.intensity.value = _vignette.intensity.min;
            }
        }    
    }
}