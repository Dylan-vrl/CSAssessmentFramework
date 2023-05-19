using CSFramework.Core;
using UnityEngine;
using CSFramework.Presets;
using static CSFramework.Core.PresettableCategory;
using System.Collections;
using System.Threading.Tasks;

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
        private bool rotating = false;

        private Quaternion[] axises;

        private void Start()
        {
            lerpDuration = Preset.lapDuration;
            lapCount = Preset.lapCount;
            waitDuration = Preset.waitDuration;

            axises = new Quaternion[3];
            axises[0] = Quaternion.Euler(180, 0, 0);
            axises[1] = Quaternion.Euler(0, 180, 0);
            axises[2] = Quaternion.Euler(0, 0, 180);
        }

        private void Update()
        {
            if ( GameStateManager.IsPlaying && !rotating)
            {
                GameStateManager.PauseGame(true);
                Begin();
            }
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
                        await Rotate360(axises[rotationTurns[k, j]]);
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

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(5);
        }

    }
}