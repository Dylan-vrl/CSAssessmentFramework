using UnityEngine;
using CSFramework.Core;

namespace CSFramework.Samples
{
    [RequireComponent(typeof(Camera))]
    public class CameraRotator : Extension<Camera, CameraRotatorPreset>
    {
        private Camera _cam;

        private void Awake()
        {
            _cam = GetComponent<Camera>();
        }

        private void Update()
        {
            _cam.transform.Rotate(Preset.RotationAxis, Preset.RotationSpeed);
        }

        public override PresettableCategory Category => PresettableCategory.Vision;
    }
}
