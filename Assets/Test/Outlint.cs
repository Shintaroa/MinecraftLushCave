using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;



public class Outlint : ScriptableRendererFeature
{
    [System.Serializable]
    public class Settings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        public Shader shader;
    }
    public Settings settings = new Settings();

    OutlintPass outlintPass;           // 定义我们创建出Pass


    public override void Create()
    {
        this.name = "Outlint";    // 模糊渲染的名字
        outlintPass = new OutlintPass(RenderPassEvent.BeforeRenderingPostProcessing, settings.shader);    // 初始化 我们的渲染层级和Shader

    }
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(outlintPass);
    }
}


public class OutlintPass : ScriptableRenderPass
{
    static readonly string RenderTag = "Post Effects";                         // 设置渲染标签

    OutlintVolume outlintvolume;                                                            // 定义组件类型
    Material biltmaterial;                                                      // 后处理材质


    public OutlintPass(RenderPassEvent evt, Shader biltshader)
    {
        renderPassEvent = evt;
        var shader = biltshader;

        if (shader == null)
        {
            Debug.LogError("没有指定Shader");
            return;
        }
        biltmaterial = CoreUtils.CreateEngineMaterial(biltshader);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (biltmaterial == null)
        {
            Debug.LogError("材质初始化失败");
            return;
        }

        if (!renderingData.cameraData.postProcessEnabled)
        {
            return;
        }

        var stack = VolumeManager.instance.stack;                          // 传入 volume
        outlintvolume = stack.GetComponent<OutlintVolume>();                     // 获取到后处理组件

        var cmd = CommandBufferPool.Get(RenderTag);    // 渲染标签

        Render(cmd, ref renderingData);                 // 调用渲染函数

        context.ExecuteCommandBuffer(cmd);              // 执行函数，回收。
        CommandBufferPool.Release(cmd);

    }

    void Render(CommandBuffer cmd, ref RenderingData renderingData)
    {
        RenderTargetIdentifier source = renderingData.cameraData.renderer.cameraColorTarget;                 // 定义RT
        RenderTextureDescriptor inRTDesc = renderingData.cameraData.cameraTargetDescriptor;
        inRTDesc.depthBufferBits = 0;                                                                          // 清除深度

        var camera = renderingData.cameraData.camera;                         // 传入摄像机
        Matrix4x4 clipToView = GL.GetGPUProjectionMatrix(camera.projectionMatrix, true).inverse;

        biltmaterial.SetColor("_Color", outlintvolume.OutlintColor.value);   // 获取value 组件的颜色

        biltmaterial.SetMatrix("_ClipToView", clipToView);   // 反向输出到Shader

        biltmaterial.SetFloat("_Scale", outlintvolume.Scale.value);
        biltmaterial.SetFloat("_DepthThreshold", outlintvolume.DepthThreshold.value);
        biltmaterial.SetFloat("_NormalThreshold", outlintvolume.NormalThreshold.value);

        biltmaterial.SetFloat("_DepthNormalThreshold", outlintvolume.DepthNormalThreshold.value);
        biltmaterial.SetFloat("_DepthNormalThresholdScale", outlintvolume.DepthNormalThresholdScale.value);

        biltmaterial.SetInt("_IsEditor", 1);
        biltmaterial.SetInt("_IsEditor", 0);
        int destination = Shader.PropertyToID("Temp1");

        // 获取一张临时RT
        cmd.GetTemporaryRT(destination, inRTDesc.width, inRTDesc.height, 0, FilterMode.Bilinear, RenderTextureFormat.DefaultHDR); //申请一个临时图像，并设置相机rt的参数进去

        cmd.Blit(source, destination);                            // 设置后处理


        cmd.Blit(destination, source, biltmaterial, 0);                            //  第二个Pass
    }
}
