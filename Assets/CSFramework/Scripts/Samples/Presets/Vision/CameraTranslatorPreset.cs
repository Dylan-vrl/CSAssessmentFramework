using UnityEngine;
using CSFramework.Core;

namespace CSFramework.Samples
{
    [CreateAssetMenu(fileName = "CameraTranslatorPreset", menuName = "Presets/Vision/Camera Translator Preset")]
    public class CameraTranslatorPreset : Preset<CameraTranslator>
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private Vector3 direction = Vector3.up;

        public float Speed => speed;
        public Vector3 Direction => direction;

        private void OnValidate()
        {
            direction = direction.normalized;
        }
    }
}