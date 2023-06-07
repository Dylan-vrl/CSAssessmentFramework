using CSFramework.Core;
using UnityEngine;
using CSFramework.Presets;
using static CSFramework.Core.PresettableCategory;

namespace CSFramework.Presettables
{
	public class RestFrames : PresettableMonoBehaviour<RestFramesPreset>
	{
        public override PresettableCategory GetCategory() => Vision;
        
        // You can access your Preset's fields: Preset.fieldName
		private bool nose;
        private GameObject nosePrefab;
        private float yPosition;
        private float zPosition;
        private float noseWidth;
        private float noseFlatness;

        private bool hat;
        private GameObject hatPrefab;

        void Awake() {
            nose = Preset.nose;
        	noseWidth = Preset.noseWidth;
        	noseFlatness = Preset.noseFlatness;
        	nosePrefab = GameObject.Find(Preset.nosePrefabName);
            if(nosePrefab != null) {
                nosePrefab.SetActive(nose);
            }
        
        	hat = Preset.hat;
        	hatPrefab = GameObject.Find(Preset.hatPrefabName);
            if(hatPrefab != null) {
                hatPrefab.SetActive(hat);
            }
        }
		
		void Start()
        {
            // SINGLE NOSE --------------------------------------------
            if (nose && nosePrefab == null)
            {
				Debug.Log("Nose Prefab is not found on the scene. It will be generated.");
                Quaternion indicatorRotation = Quaternion.Euler(60, 0, 0);
                Vector3 indicatorPosition = new Vector3(0f, -0.196f, 0.483f);
                nosePrefab = (GameObject) Instantiate(Resources.Load(Preset.nosePrefabName), indicatorPosition, indicatorRotation);
                // instantiate as a child of the main camera
                GameObject mainCamera = Object.FindObjectOfType<Camera>().gameObject;
                nosePrefab.transform.parent = mainCamera.transform;
                nosePrefab.transform.localPosition = indicatorPosition;
                nosePrefab.transform.rotation = indicatorRotation;
            }
            if ( nosePrefab != null) {
                nosePrefab.SetActive(nose);
                var noseScript = nosePrefab.GetComponent<SingleNose>();
                noseScript.NoseWidth = noseWidth;
                noseScript.NoseFlatness = noseFlatness;
            }


            // HAT --------------------------------------------
            if (hat && hatPrefab == null)
            {
                Debug.Log("Hat Prefab is not found on the scene.");
                Quaternion indicatorRotation = Quaternion.Euler(0, 0, 0);
                Vector3 indicatorPosition = new Vector3(0f, 0.12f, 0f);
                hatPrefab = (GameObject) Instantiate(Resources.Load(Preset.hatPrefabName), indicatorPosition, indicatorRotation);
                // instantiate as a child
                GameObject mainCamera = Object.FindObjectOfType<Camera>().gameObject;
                hatPrefab.transform.parent = mainCamera.transform;
                hatPrefab.transform.localPosition = indicatorPosition;
                hatPrefab.transform.rotation = indicatorRotation;
            }
            else if (hatPrefab != null)
            {
                hatPrefab.SetActive(hat);
            }
        }
	}
}