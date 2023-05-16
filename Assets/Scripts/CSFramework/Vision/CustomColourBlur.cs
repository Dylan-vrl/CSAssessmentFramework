using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[Serializable, VolumeComponentMenuForRenderPipeline("Colour Blur", typeof(UniversalRenderPipeline))]
public class CustomColourBlur : VolumeComponent, IPostProcessComponent
{
    public FloatParameter blurStrength = new FloatParameter(.5f);
    public FloatParameter brightnessThreshold = new FloatParameter(4f);
    public FloatParameter redThreshold = new FloatParameter(.5f);
    public FloatParameter greenThreshold = new FloatParameter(.5f);
    public FloatParameter blueThreshold = new FloatParameter(.5f);

    public bool IsActive() => true;
    public bool IsTileCompatible() => true;
}
