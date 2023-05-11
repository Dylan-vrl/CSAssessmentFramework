using CSFramework.Core;
using UnityEngine;
using CSFramework.Presets;
using static CSFramework.Core.PresettableCategory;

namespace CSFramework.Extensions
{
    [RequireComponent(typeof(Camera))]
	public class PersonalSensitivityTest : Extension<Camera, PersonalSensitivityTestPreset>
	{
        public override PresettableCategory GetCategory() => Experiment;

        private Animator animator;

        // You can access your Preset's fields: Preset.fieldName
        
        private void Awake()
        {
	        Camera _camera = GetComponent<Camera>();
            animator = _camera.GetComponent<Animator>();
		}

        private void Start()
        {
			//animator.clip = Preset.rotateX;
            animator.Play("RotateX");
		}
	}
}