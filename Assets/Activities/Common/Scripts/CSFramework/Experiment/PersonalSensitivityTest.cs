using CSFramework.Core;
using UnityEngine;
using CSFramework.Presets;
using static CSFramework.Core.PresettableCategory;
using System.Collections;
using System.Threading.Tasks;
using TMPro;

namespace CSFramework.Extensions
{
    [RequireComponent(typeof(Camera))]
	public class PersonalSensitivityTest : Extension<Camera, PersonalSensitivityTestPreset>
	{
        public override PresettableCategory GetCategory() => Experiment;

        // You can access your Preset's fields: Preset.fieldName

        private float lerpDuration;
        private float waitDuration;
        private int lapCount;
        private int waitBtwnAxisTime;
        private bool rotating = false;

        private Quaternion[] axises;
        private Quaternion indicatorRotation;
        private Vector3 indicatorPosition;
        private GameObject indicator;
        private GameObject[] indicatorAxises;
        private TextMeshPro indicatorText;
        private string[] axisNames;

        private void Start()
        {
            lerpDuration = Preset.lapDuration;
            lapCount = Preset.lapCount;
            waitDuration = Preset.waitDuration;
            waitBtwnAxisTime = 3;

            axises = new Quaternion[3];
            axises[0] = Quaternion.Euler(180, 0, 0);
            axises[1] = Quaternion.Euler(0, 180, 0);
            axises[2] = Quaternion.Euler(0, 0, 180);

            indicatorRotation = Quaternion.Euler(0, 180, 0);
            indicatorPosition = new Vector3(0.489f, 0.132f, 1.281f);
            indicator = (GameObject) Instantiate(Resources.Load("PitchRollYaw"), indicatorPosition, indicatorRotation);
            indicatorText = indicator.transform.GetChild(3).gameObject.GetComponent<TextMeshPro>();
            indicatorAxises = new GameObject[3];
            indicatorAxises[0] = indicator.transform.GetChild(0).gameObject;
            indicatorAxises[1] = indicator.transform.GetChild(2).gameObject;
            indicatorAxises[2] = indicator.transform.GetChild(1).gameObject;
            indicatorAxises[0].SetActive(false);
            indicatorAxises[1].SetActive(false);
            indicatorAxises[2].SetActive(false);
            
            indicatorText.text = "";
            axisNames = new string[3];
            axisNames[0] = "PITCH";
            axisNames[2] = "ROLL";
            axisNames[1] = "YAW";
        }

        private void Update()
        {
            if ( GameStateManager.IsPlaying && !rotating)
            {
                GameStateManager.TestingGame(true);
                Begin();
            }
            //GameStateManager.TestingGame(false);
        }

        private async void Begin()
        {
            rotating = true;
            int [,] rotationTurns = {{0,1,2}, {1,0,2}, {2,1,0}};
            for (int k = 0; k < rotationTurns.Length; k++) {
                await Task.Delay((int) (waitDuration*1000));
                for(int j=0; j < 3; j++)
                {
                    for (int i = 0; i < lapCount; i++)
                    {
                        int axisNo = rotationTurns[k, j];
                        // show next axis direction
                        indicatorText.text = axisNames[axisNo];
                        indicatorAxises[axisNo].SetActive(true);
                        
                        await Task.Delay(waitBtwnAxisTime*1000);
                        // remove axis direction
                        indicatorAxises[axisNo].SetActive(false);
                        indicatorText.text = "";

                        await Rotate360(axises[axisNo]);
                    }
                }
            }
        }

        async Task Rotate360(Quaternion turnAxis)
        {
            
            float timeElapsed = 0f;
            Quaternion startRotation = transform.rotation;
            Quaternion midRotation = transform.rotation * turnAxis;

            while (timeElapsed < lerpDuration)
            {
                transform.rotation = Quaternion.Slerp(startRotation, midRotation, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                await Task.Yield();
            }
            transform.rotation = midRotation;
            
            timeElapsed = 0f;
            Quaternion finalRotation = transform.rotation * turnAxis;
            while (timeElapsed < lerpDuration)
            {
                transform.rotation = Quaternion.Slerp(midRotation, finalRotation, timeElapsed / lerpDuration);
                timeElapsed += Time.deltaTime;
                await Task.Yield();
            }
            transform.rotation = startRotation;
        }
    }
}