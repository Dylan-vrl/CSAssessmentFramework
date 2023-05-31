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
        	nosePrefab = GameObject.Find(Preset.nosePrefabName);
            nosePrefab.SetActive(nose);
        	yPosition = Preset.yPosition;
        	zPosition = Preset.zPosition;
        	noseWidth = Preset.noseWidth;
        	noseFlatness = Preset.noseFlatness;

        	hat = Preset.hat;
        	hatPrefab = GameObject.Find(Preset.hatPrefabName);
            hatPrefab.SetActive(hat);
        }
		
		void Start()
        {
            // SINGLE NOSE --------------------------------------------
            if (nose && nosePrefab == null)
            {
				Debug.Log("Nose Prefab is not found on the scene.");
                Quaternion indicatorRotation = Quaternion.Euler(60, 0, 0);
                Vector3 indicatorPosition = new Vector3(0f, -0.24f, 0.57f);
                var indicator = (GameObject)Instantiate(Resources.Load(Preset.nosePrefabName), indicatorPosition, indicatorRotation);
                // instantiate as a child
                indicator.transform.parent = transform;
                indicator.transform.localPosition = indicatorPosition;
                indicator.transform.rotation = indicatorRotation;
            } else if ( nosePrefab != null) {
                nosePrefab.SetActive(nose);
                var noseScript = nosePrefab.GetComponent<SingleNose>();
                noseScript.YPosition = yPosition;
                noseScript.ZPosition = zPosition;
                noseScript.NoseWidth = noseWidth;
                noseScript.NoseFlatness = noseFlatness;
            }


            // HAT --------------------------------------------
            if (hat && hatPrefab == null)
            {
                Debug.Log("Hat Prefab is not found on the scene.");
                Quaternion indicatorRotation = Quaternion.Euler(0, 0, 0);
                Vector3 indicatorPosition = new Vector3(0f, 0.12f, 0f);
                var indicator = (GameObject)Instantiate(Resources.Load(Preset.hatPrefabName), indicatorPosition, indicatorRotation);
                // instantiate as a child
                indicator.transform.parent = transform;
                indicator.transform.localPosition = indicatorPosition;
                indicator.transform.rotation = indicatorRotation;
            }
            else if (hatPrefab != null)
            {
                hatPrefab.SetActive(hat);
            }
        }
	}
}