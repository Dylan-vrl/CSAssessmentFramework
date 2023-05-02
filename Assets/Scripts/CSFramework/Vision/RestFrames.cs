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
		
		void Start()
        {
			nose = Preset.nose;
        	nosePrefab = Preset.nosePrefab;
        	yPosition = Preset.yPosition;
        	zPosition = Preset.zPosition;
        	noseWidth = Preset.noseWidth;
        	noseFlatness = Preset.noseFlatness;

        	hat = Preset.hat;
        	hatPrefab = Preset.hatPrefab;

            // SINGLE NOSE --------------------------------------------
            if (nose && nosePrefab == null)
            {
				Debug.Log("Single Nose Prefab is not found on the scene.");
				nose = false;
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
                hat = false;
            }
            else if (hatPrefab != null)
            {
                hatPrefab.SetActive(hat);
            }
        }
	}
}