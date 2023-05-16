using CSFramework.Core;
using UnityEngine;
using static UnityEngine.Debug;
using CSFramework.Presets;
using static CSFramework.Core.PresettableCategory;
using UnityEngine.Rendering;
using CSFramework.Presettables;
using System.Collections;

namespace CSFramework.Extensions
{
    [RequireComponent(typeof(Volume))]
	public class ColorBlur : Extension<Volume, ColorBlurPreset>
	{
        public override PresettableCategory GetCategory() => Vision;

        private Volume _volume;
        private CustomColourBlur _customColourBlur;

        private CharacterController _xrChara = null;
        private Vector3 _lastPos;
        private Quaternion _lastRot;
        private int _changing;

        // You can access your Preset's fields: Preset.fieldName

        private void Awake()
        {
			// You might want to get your component in another way
	        _volume = GetComponent<Volume>();
		}

        private void OnEnable()
        {

            if (_volume == null)
            {
                //Debug.Log("No Post Processing set");
                return;
            }
            if (!_volume.profile.TryGet<CustomColourBlur>(out _customColourBlur))
            {
                //Debug.Log("No Custom Color Blur found");
                return;
            }

            _customColourBlur.active = true;
            FloatParameter temp = _customColourBlur.blurStrength;
            temp.value = Preset.blurStrength;
            temp = _customColourBlur.brightnessThreshold;
            temp.value = Preset.brightnessThreshold;
            temp = _customColourBlur.redThreshold;
            temp.value = Preset.redThreshold;
            temp = _customColourBlur.greenThreshold;
            temp.value = Preset.greenThreshold;
            temp = _customColourBlur.blueThreshold;
            temp.value = Preset.blueThreshold;

        }

        // Start is called before the first frame update
        void Start()
        {
            _xrChara = GameHandler.Instance.XROrigin.GetComponent<CharacterController>();
            _lastRot = _xrChara.transform.rotation;
            _lastPos = _xrChara.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (_xrChara != null && GameHandler.State == GameHandler.StateType.Playing)
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
                    _changing = (m || t) ? 1 : -1;
                    // toggle color blur
                }
            }
        }

        private void OnDisable()
        {
            if (_volume != null && _customColourBlur != null)
            {
                _customColourBlur.active = false;
            }
        }
    }
}