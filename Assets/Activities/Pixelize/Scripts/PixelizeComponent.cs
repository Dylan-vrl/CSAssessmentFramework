using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[VolumeComponentMenuForRenderPipeline("Custom/Pixelize", typeof(UniversalRenderPipeline))]
public class PixelizeComponent : VolumeComponent, IPostProcessComponent
{
    public ClampedIntParameter screenHeight = new(0, 0, 2160);

    public void Init(int screenHeight)
    {
        this.screenHeight.value = screenHeight;
    }
    
    public bool IsActive()
    {
        return screenHeight.value > 0;
    }

    public bool IsTileCompatible()
    {
        return false;
    }
}
