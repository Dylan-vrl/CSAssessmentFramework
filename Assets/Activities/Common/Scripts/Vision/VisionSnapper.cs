using CSFramework.Core;
using CSFramework.Presets;
using CSFramework.Presettables;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using static CSFramework.Core.PresettableCategory;
//using static System.Net.Mime.MediaTypeNames;

namespace CSFramework.Extensions
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Volume))]
	public class VisionSnapper : Extension<Volume, VisionSnapperPreset>
	{
        public override PresettableCategory GetCategory() => Vision;

        private Volume postProcessing;

        // You can access your Preset's fields: Preset.fieldName
        private UnityEngine.Rendering.Universal.ColorAdjustments _colorAdjustments;
        private ColorParameter colorFilter;

        private CharacterController _xrChara = null;
        private Quaternion _lastRot;
        private int _changing;
        private Coroutine _changeColourRoutine;
        
        private void Awake()
        {
			// You might want to get your component in another way
	        postProcessing = GetComponent<Volume>();
		}

        private void Start()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            
            _xrChara = ExperimentController.Instance.XROrigin.GetComponent<CharacterController>();
            _lastRot = _xrChara.transform.rotation;
        }

        private void OnEnable()
        {

            if (postProcessing == null)
            {
                Debug.LogWarning("No Post Processing set");
                return;
            }
            if (!postProcessing.profile.TryGet(out _colorAdjustments))
            {
                Debug.LogWarning("No Color Adjustments found");
                return;
            }

            _colorAdjustments.active = true;
            colorFilter = _colorAdjustments.colorFilter;
            colorFilter.overrideState = true;
        }

        // Update is called once per frame
        void Update()
        {
            if (_xrChara != null && GameStateManager.State == GameStateManager.GameState.Playing)
            {
                var t = false;

                Quaternion rot = _xrChara.transform.rotation;
                Quaternion deltaRot = rot * Quaternion.Inverse(_lastRot);
                var eulerRot = deltaRot.eulerAngles;
                Vector3 angularVelocity = eulerRot / Time.fixedDeltaTime;

                // If not already turning
                if(!t) {
                    t = angularVelocity.magnitude > 0;
                }

                _lastRot = rot;
                if (_changing != (t ? 1 : -1))
                {
                    if (_changeColourRoutine != null) StopCoroutine(_changeColourRoutine);
                    _changeColourRoutine = StartCoroutine(ChangeColour(t));
                }

            }
        }

        private IEnumerator ChangeColour(bool turning)
        {
            float initialTime = Time.time;
            if (turning)
            {
                float f = 0f;
                Color fadingColor = new Color(0,0,0,0); //black transparent
                while(f > 1f){
                    f = Mathf.Clamp01((Time.time - initialTime) + 0.2f);
                    fadingColor.a = f;
                    colorFilter.value = fadingColor;
                }
                colorFilter.value = Color.black;
            }
            else
            {
                colorFilter.value = Color.white;
            }
            _changing = turning ? 1 : -1;
            yield return null;
        }

        private void OnDisable()
        {
            if (postProcessing != null && _colorAdjustments != null)
            {
                _colorAdjustments.active = false;

            }
        }
	}
}