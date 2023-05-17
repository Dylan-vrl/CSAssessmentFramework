using static CSFramework.Core.PresettableCategory;
using System.Collections;
using CSFramework.Core;
using CSFramework.Presets;
using CSFramework.Presettables;
using UnityEngine;
using UnityEngine.Rendering;

namespace CSFramework.Extensions
{
    [RequireComponent(typeof(Volume))]
	public class ColorManipulation : Extension<Volume, ColorManipulationPreset>
	{
        public override PresettableCategory GetCategory() => Vision;

        private Volume postProcessing;

        private UnityEngine.Rendering.Universal.ColorAdjustments _colorAdjustments;
        private ClampedFloatParameter contrast;
        private ClampedFloatParameter saturation;
        private UnityEngine.Rendering.Universal.ColorCurves _colorCurves;

        private CharacterController _xrChara = null;
        private Vector3 _lastPos;
        private Quaternion _lastRot;
        private int _changing;
        private Coroutine _changeColourRoutine;

        private Keyframe[] keyFrames;
        private TextureCurveParameter redTex;
        private TextureCurveParameter greenTex;
        private TextureCurveParameter blueTex;
        private TextureCurveParameter masterTex;

        // You can access your Preset's fields: Preset.fieldName
        
        private void Awake()
        {
			// You might want to get your component in another way
	        postProcessing = GetComponent<Volume>();
		}

        private void OnEnable()
        {
            
            if (postProcessing == null)
            {
                Debug.LogWarning("No Post Processing set");
                return;
            }
            if (!postProcessing.profile.TryGet(out _colorCurves))
            {
                Debug.LogWarning("No Color Curves found");
                return;
            }
            if (!postProcessing.profile.TryGet(out _colorAdjustments))
            {
                Debug.LogWarning("No Color Adjustments found");
                return;
            }

            _colorAdjustments.active = Preset.contrastSaturation;
            contrast = _colorAdjustments.contrast;
            contrast.overrideState = Preset.contrastSaturation;
            saturation = _colorAdjustments.saturation;
            saturation.overrideState = Preset.contrastSaturation;

            _colorCurves.active = Preset.hueManipulation;
            masterTex = _colorCurves.master;
            masterTex.overrideState = Preset.hueManipulation;
            redTex = _colorCurves.red;
            redTex.overrideState = Preset.hueManipulation;
            greenTex = _colorCurves.green;
            greenTex.overrideState = Preset.hueManipulation;
            blueTex = _colorCurves.blue;
            blueTex.overrideState = Preset.hueManipulation;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            _xrChara = ExperimentController.Instance.XROrigin.GetComponent<CharacterController>();
            _lastRot = _xrChara.transform.rotation;
            _lastPos = _xrChara.transform.position;

            keyFrames = new Keyframe[8];
            keyFrames[0] = new Keyframe(1, Preset.redDegradation);
            keyFrames[1] = new Keyframe(1, Preset.greenDegradation);
            keyFrames[2] = new Keyframe(1, Preset.blueDegradation);
            keyFrames[3] = new Keyframe(1, Preset.whiteDegradation);
            // intial values
            keyFrames[4] = redTex.value[1];
            keyFrames[5] = greenTex.value[1];
            keyFrames[6] = blueTex.value[1];
            keyFrames[7] = masterTex.value[1];
        }

        // Update is called once per frame
        void Update()
        {
            if (_xrChara != null && GameStateManager.State == GameStateManager.GameState.Playing)
            {
                var t = false;
                var m = false;

                // if moving fast
                var v = _xrChara.velocity.magnitude;
                Vector3 pos = _xrChara.transform.position;
                m = v > 0.3 && pos != _lastPos;

                // If snap turning
                Quaternion rot = _xrChara.transform.rotation;
                Quaternion deltaRot = rot * Quaternion.Inverse(_lastRot);
                var eulerRot = deltaRot.eulerAngles;
                Vector3 angularVelocity = eulerRot / Time.fixedDeltaTime;
                t = angularVelocity.magnitude > 0;

                // update last values
                _lastPos = pos;
                _lastRot = rot;
                if (_changing != (m || t ? 1 : -1))
                {
                    if (_changeColourRoutine != null) StopCoroutine(_changeColourRoutine);
                    _changeColourRoutine = StartCoroutine(ChangeColour(m || t, Time.time));
                }
            }
        }

        private IEnumerator ChangeColour(bool moving, float time)
        {
            _changing = moving ? 1 : -1;
            if (Preset.hueManipulation)
            {
                if (moving)
                {
                    redTex.value.MoveKey(1, keyFrames[0]);
                    greenTex.value.MoveKey(1, keyFrames[1]);
                    blueTex.value.MoveKey(1, keyFrames[2]);
                    masterTex.value.MoveKey(1, keyFrames[3]);
                }
                else
                {
                    redTex.value.MoveKey(1, keyFrames[4]);
                    greenTex.value.MoveKey(1, keyFrames[5]);
                    blueTex.value.MoveKey(1, keyFrames[6]);
                    masterTex.value.MoveKey(1, keyFrames[7]);
                }
            }
            if(Preset.contrastSaturation)
            {
                var f = 0f;
                var goalCont = 0f;
                var goalSat = 0f;
                while (f < 1)
                {
                    f = Mathf.Clamp01(Time.time - time + 0.2f);
                    if (moving)
                    {
                        goalCont = Preset.contrastStrength;
                        goalSat = Preset.saturationStrength;
                    } else
                    {
                        goalCont = 0f;
                        goalSat = 0f;
                    }
                    contrast.value = Mathf.Lerp(_colorAdjustments.contrast.value, goalCont, f);
                    saturation.value = Mathf.Lerp(_colorAdjustments.saturation.value, goalSat, f);
                    yield return null;
                }
            }
            yield return null;
        }

        private void OnDisable()
        {
            if (postProcessing != null && _colorCurves != null )
            {
                _colorCurves.active = false;
            }
        }
	}
}