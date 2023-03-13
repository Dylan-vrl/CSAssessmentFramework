using UnityEngine;
using CSFramework.Core;

namespace CSFramework.Samples
{
    [RequireComponent(typeof(Camera))]
    public class CameraTranslator : Extension<Camera, CameraTranslatorPreset>
    {
        private Camera _cam;

        private void Awake()
        {
            _cam = GetComponent<Camera>();
        }

        private void Update()
        {
            _cam.transform.Translate(Preset.Direction * Preset.Speed);
        }

        public override PresettableCategory Category => PresettableCategory.Vision;
    }
}