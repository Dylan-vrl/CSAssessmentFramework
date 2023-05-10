using CSFramework.Core;
using UnityEngine;
using CSFramework.Presets;
using static CSFramework.Core.PresettableCategory;
using static System.Net.Mime.MediaTypeNames;

namespace CSFramework.Presettables
{
	[RequireComponent(typeof(AudioSource))]
	public class BackgroundMusic : PresettableMonoBehaviour<BackgroundMusicPreset>
	{
        public override PresettableCategory GetCategory() => Environment;

		private AudioSource audioSource;
        private bool isPlaying;
        
        // You can access your Preset's fields: Preset.fieldName
		void Awake()
		{
			audioSource = GetComponent<AudioSource>();
			audioSource.clip = Preset.musicLoop;
		}

		void Start()
		{
            isPlaying = false;
		}

        void Update()
        {
            if (GameHandler.State != GameHandler.StateType.Playing)
            {
                return;
            } 
            if (!isPlaying)
            {
                isPlaying = true;
                if (Preset.musicStart.Enabled)
                {
                    audioSource.PlayOneShot(Preset.musicStart.Value);
                    audioSource.PlayScheduled(AudioSettings.dspTime + Preset.musicStart.Value.length);
                } else
                {
                    audioSource.loop = true;
                    audioSource.Play();
                }
                
            }
            
        }
    }
}