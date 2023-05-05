using CSFramework.Core;
using UnityEngine;
using CSFramework.Presets;
using static CSFramework.Core.PresettableCategory;

namespace CSFramework.Presettables
{
	[RequireComponent(typeof(AudioSource))]
	public class BackgroundMusic : PresettableMonoBehaviour<BackgroundMusicPreset>
	{
        public override PresettableCategory GetCategory() => Environment;

		private AudioSource audioSource;
        
        // You can access your Preset's fields: Preset.fieldName
		void Awake()
		{
			audioSource = GetComponent<AudioSource>();
			audioSource.clip = Preset.musicLoop;
		}

		void Start()
		{
			audioSource.PlayOneShot(Preset.musicStart);
			audioSource.PlayScheduled(AudioSettings.dspTime + Preset.musicStart.length);
		}
	}
}