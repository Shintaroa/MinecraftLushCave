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

    OutlintPass outlintPass;           // �������Ǵ�����Pass


    public override void Create()
    {
        this.name = "Outlint";    // ģ����Ⱦ������
        outlintPass = new OutlintPass(RenderPassEvent.BeforeRenderingPostProcessing, settings.shader);    // ��ʼ�� ���ǵ���Ⱦ�㼶��Shader

    }
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(outlintPass);
    }
}


public class OutlintPass : ScriptableRenderPass
{
    static readonly string RenderTag = "Post Effects";                         // ������Ⱦ��ǩ

    OutlintVolume outlintvolume;                                                            // �����������
    Material biltmaterial;                                                      // �������


    public OutlintPass(RenderPassEvent evt, Shader biltshader)
    {
        renderPassEvent = evt;
        var shader = biltshader;

        if (shader == null)
        {
            Debug.LogError("û��ָ��Shader");
            return;
        }
        biltmaterial = CoreUtils.CreateEngineMaterial(biltshader);
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (biltmaterial == null)
        {
            Debug.LogError("���ʳ�ʼ��ʧ��");
            return;
        }

        if (!renderingData.cameraData.postProcessEnabled)
        {
            return;
        }

        var stack = VolumeManager.instance.stack;                          // ���� volume
        outlintvolume = stack.GetComponent<OutlintVolume>();                     // ��ȡ���������

        var cmd = CommandBufferPool.Get(RenderTag);    // ��Ⱦ��ǩ

        Render(cmd, ref renderingData);                 // ������Ⱦ����

        context.ExecuteCommandBuffer(cmd);              // ִ�к��������ա�
        CommandBufferPool.Release(cmd);

    }

    void Render(CommandBuffer cmd, ref RenderingData renderingData)
    {
        RenderTargetIdentifier source = renderingData.cameraData.renderer.cameraColorTarget;                 // ����RT
        RenderTextureDescriptor inRTDesc = renderingData.cameraData.cameraTargetDescriptor;
        inRTDesc.depthBufferBits = 0;                                                                          // ������

        var camera = renderingData.cameraData.camera;                         // ���������
        Matrix4x4 clipToView = GL.GetGPUProjectionMatrix(camera.projectionMatrix, true).inverse;

        biltmaterial.SetColor("_Color", outlintvolume.OutlintColor.value);   // ��ȡvalue �������ɫ

        biltmaterial.SetMatrix("_ClipToView", clipToView);   // ���������Shader

        biltmaterial.SetFloat("_Scale", outlintvolume.Scale.value);
        biltmaterial.SetFloat("_DepthThreshold", outlintvolume.DepthThreshold.value);
        biltmaterial.SetFloat("_NormalThreshold", outlintvolume.NormalThreshold.value);

        biltmaterial.SetFloat("_DepthNormalThreshold", outlintvolume.DepthNormalThreshold.value);
        biltmaterial.SetFloat("_DepthNormalThresholdScale", outlintvolume.DepthNormalThresholdScale.value);

        biltmaterial.SetInt("_IsEditor", 1);
        biltmaterial.SetInt("_IsEditor", 0);
        int destination = Shader.PropertyToID("Temp1");

        // ��ȡһ����ʱRT
        cmd.GetTemporaryRT(destination, inRTDesc.width, inRTDesc.height, 0, FilterMode.Bilinear, RenderTextureFormat.DefaultHDR); //����һ����ʱͼ�񣬲��������rt�Ĳ�����ȥ

        cmd.Blit(source, destination);                            // ���ú���


        cmd.Blit(destination, source, biltmaterial, 0);                            //  �ڶ���Pass
    }
}
