using System;
using CSFramework.Core;
using UnityEngine;
using CSFramework.Presets;
using UnityEngine.Rendering;
using static CSFramework.Core.PresettableCategory;
/*
namespace CSFramework.Extensions
{
	[RequireComponent(typeof(Volume))]
	public class Pixelize : Extension<Volume, PixelizePreset>
	{
		public override PresettableCategory GetCategory() => Vision;

		private Volume _volume;

		private void Awake()
		{
			_volume = GetComponent<Volume>();
		}

		private void OnEnable()
		{
			// If no PixelizeComponent found, create a new one
			if (!_volume.profile.TryGet<PixelizeComponent>(out var pixelizeComponent))
				pixelizeComponent = _volume.profile.Add<PixelizeComponent>();
			
			// Set the PixelizeComponent active
			pixelizeComponent.active = true;
			foreach (var param in pixelizeComponent.parameters)
				param.overrideState = true;
			
			// Set the height to the preset one
			pixelizeComponent.screenHeight.value = Preset.ScreenHeight;
		}
	}
}
*/