using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorBlurFeature : ScriptableRendererFeature
{
    private BlurPass blurPass;

    public override void Create()
    {
        blurPass = new BlurPass();
    }
    
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(blurPass);
    }


    class BlurPass: ScriptableRenderPass
    {
        private Material _mat;
        private int blurID = Shader.PropertyToID("_Temp");
        private RenderTargetIdentifier src, blur;

        public void TintPass()
        {
            if(!_mat)
            {
                _mat = CoreUtils.CreateEngineMaterial("Custom/SaliencyBlur");
            }
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor desc = renderingData.cameraData.cameraTargetDescriptor;
            src = renderingData.cameraData.renderer.cameraColorTarget;
            cmd.GetTemporaryRT(blurID, desc, FilterMode.Bilinear);
            blur = new RenderTargetIdentifier();
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer commandBuffer = CommandBufferPool.Get("ColorBlurFeature");
            VolumeStack volumes = VolumeManager.instance.stack;
            CustomColourBlur blurdata = volumes.GetComponent<CustomColourBlur>();
            if(blurdata.IsActive())
            {
                float sigma = blurdata.blurStrength.value;
                float[] kernel = new float[121];
                //initialize to some 
                for (int x = 0; x < 11; x++)
                {
                    for (int y = 0; y < 11; y++)
                    {
                        kernel[y * 11 + x] = GaussianFunction(x - 5.0f, y - 5.0f, sigma); //update kernel
                    }
                }
                //calculate sum for later
                float kernelSum = 0;
                for (int i = 0; i < kernel.Length; i++)
                {
                    kernelSum += kernel[i];
                }

                for (int i = 0; i < kernel.Length; i++)
                {
                    kernel[i] *= (1f / kernelSum);
                }

                _mat.SetFloatArray("_kernel", kernel);
                _mat.SetFloat("_kernelSum", kernelSum);
                _mat.SetFloat("_brightnessThreshold", blurdata.brightnessThreshold.value);
                _mat.SetFloat("_redThreshold", blurdata.redThreshold.value);
                _mat.SetFloat("_greenThreshold", blurdata.greenThreshold.value);
                _mat.SetFloat("_blueThreshold", blurdata.blueThreshold.value);
                _mat.SetFloat("_darkSaliency", 0);

                Blit(commandBuffer, src, blur, _mat, 0);
                Blit(commandBuffer, blur, src);
            }
            context.ExecuteCommandBuffer(commandBuffer);
            CommandBufferPool.Release(commandBuffer);
        }

        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(blurID);
        }

        private float GaussianFunction(float x, float y, float sigma)
        {
            float p1 = 1f / ((2f * Mathf.PI) * Mathf.Pow(sigma, 2f));
            float eExponent = -(Mathf.Pow(x, 2) + Mathf.Pow(y, 2)) / (2 * Mathf.Pow(sigma, 2));
            float answer = p1 * Mathf.Exp(eExponent);
            return answer;
        }
    }
}
