using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlintVolume : VolumeComponent, IPostProcessComponent
{
    [Tooltip("��Ե��ɫ")]
    public ColorParameter OutlintColor = new ColorParameter(Color.white);
    [Tooltip("��Ե����С")]
    public ClampedFloatParameter Scale = new ClampedFloatParameter(1f, 0f, 10f);
    [Tooltip("���")]
    public ClampedFloatParameter DepthThreshold = new ClampedFloatParameter(0.2f, 0f, 10f);

    [Tooltip("�������")]
    public ClampedFloatParameter NormalThreshold = new ClampedFloatParameter(0.4f, 0f, 1f);
    public ClampedFloatParameter DepthNormalThreshold = new ClampedFloatParameter(0.5f, 0f, 1f);
    public ClampedFloatParameter DepthNormalThresholdScale = new ClampedFloatParameter(7f, 0f, 10f);


    public bool IsActive() => Scale.value > 0;

    public bool IsTileCompatible() => false;

}
