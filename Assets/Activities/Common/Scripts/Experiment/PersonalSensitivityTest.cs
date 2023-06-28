using CSFramework.Core;
using UnityEngine;
using CSFramework.Presets;
using static CSFramework.Core.PresettableCategory;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

namespace CSFramework.Extensions
{
    [RequireComponent(typeof(Camera))]
	public class PersonalSensitivityTest : Extension<Camera, PersonalSensitivityTestPreset>
	{
        public override PresettableCategory GetCategory() => Experiment;

        // preset variables
        private float lerpDuration;
        private float waitDuration;
        private int lapCount;
        private float waitBtwnAxisTime;
        private float offset;

        private Quaternion[] axises;
        private Vector3[] linAxises;
        private bool started = false;
        private static float MIN_DIST = 5f;

        //indicator variables
        private Quaternion indicatorRotation;
        private Vector3 indicatorPosition;
        private GameObject indicator;
        private GameObject[] indicatorAxises;
        private TextMeshPro indicatorText;
        private string[] axisNames;


        private void Start()
        {
            // get preset values
            lerpDuration = Preset.lapDurationPerAxis;
            lapCount = Preset.lapsPerAxis;
            waitDuration = Preset.waitDurationBtw3AxisTurns;
            waitBtwnAxisTime = Preset.waitDurationBtwEachTurn;
            offset = Preset.linearDistance < MIN_DIST ? MIN_DIST : Preset.linearDistance;

            // disable tracked pose driver
            TrackedPoseDriver trackedPoseDriver = GetComponent(typeof(TrackedPoseDriver)) as TrackedPoseDriver;
            if(trackedPoseDriver != null) {
                trackedPoseDriver.enabled = false;
            }
            
            // prepare axis vectors
            axises = new Quaternion[3];
            axises[0] = Quaternion.Euler(180, 0, 0);
            axises[1] = Quaternion.Euler(0, 180, 0);
            axises[2] = Quaternion.Euler(0, 0, 180);
            linAxises = new Vector3[3];
            linAxises[0] = new Vector3(1,0,0);
            linAxises[1] = new Vector3(0,1,0);
            linAxises[2] = new Vector3(0,0,1);

            PrepareIndicator();
        }

        private void Update()
        {
            if ( GameStateManager.IsPlaying && !started)
            {
                GameStateManager.PauseGame(true);
                Begin();
            }
            if( !Preset.insideGameScene && !started) {
                Begin();
            }
        }

        private async void Begin()
        {
            started = true;
            int [,] rotationTurns = {{0,1,2}, {1,0,2}, {0,2,1}};
            int rot = Preset.rotationalTest ? 1: 0;
            int lin = Preset.linearTest ? 1 : 0;
            int multip = rot + lin;

            for (int k = 0; k < 3 * multip; k++) {
                await Task.Delay((int) (waitDuration*1000));
                for(int j=0; j < 3; j++)
                {
                    for (int i = 0; i < lapCount; i++)
                    {
                        int axisNo = rotationTurns[k, j];
                        int indNo = (Preset.linearTest && k < 3) ? axisNo + 3: axisNo;
                        // show next axis direction
                        indicatorText.text = axisNames[indNo];
                        indicatorAxises[indNo].SetActive(true);
                        
                        await Task.Delay((int) (waitBtwnAxisTime*1000));
                        // remove axis direction
                        indicatorAxises[indNo].SetActive(false);
                        indicatorText.text = "";

                        if(Preset.linearTest && k < 3) {
                            await Linear(linAxises[axisNo]);
                        } else {
                            await Rotate360(axises[axisNo]);
                        }
                    }
                }
            }
            GameStateManager.LoadMainMenu();
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

        async Task Linear(Vector3 axis)
        {
            float timeElapsed = 0f;
            Vector3 startPosition = transform.position;
            if(Preset.insideGameScene && axis == Vector3.up) {
                Vector3 rightPosition = transform.position + (axis * offset * 2) ;
                while (timeElapsed < lerpDuration)
                {
                    transform.position = startPosition + axis * Mathf.Lerp(0, offset, timeElapsed / lerpDuration);
                    timeElapsed += Time.deltaTime;
                    await Task.Yield();
                }
                transform.position = rightPosition;

                timeElapsed = 0f;
                while (timeElapsed < lerpDuration)
                {
                    transform.position = rightPosition - axis * Mathf.Lerp(0, offset, timeElapsed / lerpDuration);
                    timeElapsed += Time.deltaTime;
                    await Task.Yield();
                }
            } else {
                Vector3 rightPosition = transform.position + (axis * offset) ;
                Vector3 leftPosition = transform.position - (axis * offset) ;

                while (timeElapsed < lerpDuration / 2)
                {
                    transform.position = startPosition + axis * Mathf.Lerp(0, offset, timeElapsed / (lerpDuration/2));
                    timeElapsed += Time.deltaTime;
                    await Task.Yield();
                }
                transform.position = rightPosition;
                
                timeElapsed = 0f;
                while (timeElapsed < lerpDuration)
                {
                    transform.position = rightPosition - axis * Mathf.Lerp(0, offset * 2, timeElapsed / lerpDuration);
                    timeElapsed += Time.deltaTime;
                    await Task.Yield();
                }
                transform.position = leftPosition;

                timeElapsed = 0f;
                while (timeElapsed < lerpDuration / 2)
                {
                    transform.position = leftPosition + axis * Mathf.Lerp(0, offset, timeElapsed / (lerpDuration/2));
                    timeElapsed += Time.deltaTime;
                    await Task.Yield();
                }
            }
            transform.position = startPosition;
        }

        private void PrepareIndicator() {
            // instantiate indicator as a child of camera
            indicatorRotation = Quaternion.Euler(0, 180, 0);
            indicatorPosition = new Vector3(0.008f, 0.052f, 1.281f);
            indicator = (GameObject) Instantiate(Resources.Load("PitchRollYaw"), indicatorPosition, indicatorRotation);
            indicator.transform.parent = transform;
            indicator.transform.localPosition = indicatorPosition;
            indicator.transform.rotation = indicatorRotation;
            
            // set and disactive all axis
            indicatorAxises = new GameObject[6];
            for( int i= 0; i<6; i++){
                indicatorAxises[i] = indicator.transform.GetChild(i).gameObject;
                indicatorAxises[i].SetActive(false);
            }
            
            //prepare indicator text field
            indicatorText = indicator.transform.GetChild(6).gameObject.GetComponent<TextMeshPro>();
            indicatorText.text = "";
            axisNames = new string[6];
            axisNames[0] = "PITCH";
            axisNames[1] = "YAW";
            axisNames[2] = "ROLL";
            axisNames[3] = "LATERAL";
            axisNames[4] = "VERTICAL";
            axisNames[5] = "LONGITUDINAL";
        }
    }
}