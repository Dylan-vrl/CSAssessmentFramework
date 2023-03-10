using UnityEngine;
using CSFramework.Core;

namespace CSFramework.Samples
{
    [CreateAssetMenu(fileName = "CameraRotatorPreset", menuName = "Presets/Vision/Camera Rotator Preset")]
    public class CameraRotatorPreset : Preset<CameraRotator>
    {
        [SerializeField] private float rotationSpeed = 1f;
        [SerializeField] private Vector3 rotationAxis = Vector3.up;

        public float RotationSpeed => rotationSpeed;
        public Vector3 RotationAxis => rotationAxis;

        private void OnValidate()
        {
            rotationAxis = rotationAxis.normalized;
        }
    }
}
