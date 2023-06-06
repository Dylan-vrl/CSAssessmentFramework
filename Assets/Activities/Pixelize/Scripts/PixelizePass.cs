
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// ReSharper disable InconsistentNaming

public class PixelizePass : ScriptableRenderPass
{
    private RenderTargetIdentifier colorBuffer, pixelBuffer;
    private int pixelBufferID = Shader.PropertyToID("_PixelBuffer");

    private Material material;
    private int pixelScreenHeight, pixelScreenWidth;
    private static readonly int BlockCount = Shader.PropertyToID("_BlockCount");
    private static readonly int BlockSize = Shader.PropertyToID("_BlockSize");
    private static readonly int HalfBlockSize = Shader.PropertyToID("_HalfBlockSize");

    public PixelizePass(Material pixelizeMaterial)
    {
        this.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        material = pixelizeMaterial;
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        colorBuffer = renderingData.cameraData.renderer.cameraColorTarget;
        RenderTextureDescriptor descriptor = renderingData.cameraData.cameraTargetDescriptor;

        VolumeStack stack = VolumeManager.instance.stack;
        var pixelizeEffect = stack.GetComponent<PixelizeComponent>();

        // If inactive (height <= 0) => do not apply effect (screen height = current resolution)
        if (!pixelizeEffect.IsActive()) 
            pixelizeEffect.Init(renderingData.cameraData.camera.scaledPixelHeight);
        
        pixelScreenHeight = pixelizeEffect.screenHeight.value;
        pixelScreenWidth = (int)(pixelScreenHeight * renderingData.cameraData.camera.aspect + 0.5f);

        material.SetVector(BlockCount, new Vector2(pixelScreenWidth, pixelScreenHeight));
        material.SetVector(BlockSize, new Vector2(1.0f / pixelScreenWidth, 1.0f / pixelScreenHeight));
        material.SetVector(HalfBlockSize, new Vector2(0.5f / pixelScreenWidth, 0.5f / pixelScreenHeight));

        descriptor.height = pixelScreenHeight;
        descriptor.width = pixelScreenWidth;

        cmd.GetTemporaryRT(pixelBufferID, descriptor, FilterMode.Point);
        pixelBuffer = new RenderTargetIdentifier(pixelBufferID);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get();
        using (new ProfilingScope(cmd, new ProfilingSampler("Pixelize Pass")))
        {
            Blit(cmd, colorBuffer, pixelBuffer, material);
            Blit(cmd, pixelBuffer, colorBuffer);
        }

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        if (cmd == null) throw new System.ArgumentNullException("cmd");
        cmd.ReleaseTemporaryRT(pixelBufferID);
    }

}