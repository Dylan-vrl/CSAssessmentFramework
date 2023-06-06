using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelizeFeature : ScriptableRendererFeature
{
    [SerializeField] 
    private Shader pixelizeShader;

    private Material pixelizeMaterial;
    private PixelizePass pass;

    public override void Create()
    {
        pixelizeMaterial = CoreUtils.CreateEngineMaterial(pixelizeShader);
        pass = new PixelizePass(pixelizeMaterial);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // Disable shader in scene view
#if UNITY_EDITOR
        if (renderingData.cameraData.isSceneViewCamera) return;
#endif
        renderer.EnqueuePass(pass);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            pass.ConfigureInput(ScriptableRenderPassInput.Color);
        }
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(pixelizeMaterial);
    }
}
