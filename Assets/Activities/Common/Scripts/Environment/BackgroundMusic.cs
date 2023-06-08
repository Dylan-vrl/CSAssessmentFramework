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
            audioSource.volume = .2f;
            audioSource.priority = 200;
		}

		void Start()
		{
            isPlaying = false;
		}

        void Update()
        {
            if (GameStateManager.State != GameStateManager.GameState.Playing)
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
                    audioSource.Play();
                }
                audioSource.loop = true;
            }
            
        }
    }
}