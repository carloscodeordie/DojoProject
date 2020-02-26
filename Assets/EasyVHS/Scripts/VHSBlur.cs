using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
public class VHSBlur : UnityStandardAssets.ImageEffects.PostEffectsBase
{

    [Range(0.0f, 10.0f)]
    public float blurAmount = 1.0f;

    [Range(1.0f, 10.0f)]
    public float channelDif = 4.0f;

    public Shader blurShader = null;
    private Material blurMaterial = null;

    public override bool CheckResources()
    {
        CheckSupport(false);

        blurMaterial = CheckShaderAndCreateMaterial(blurShader, blurMaterial);

        if (!isSupported)
            ReportAutoDisable();
        return isSupported;
    }

    public void OnDisable()
    {
        if (blurMaterial)
            DestroyImmediate(blurMaterial);
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (CheckResources() == false)
        {
            Graphics.Blit(source, destination);
            return;
        }

        blurMaterial.SetFloat("_amount", blurAmount);
        blurMaterial.SetFloat("_width", source.width);
        blurMaterial.SetFloat("_height", source.height);
        blurMaterial.SetFloat("_channelDif", channelDif);
        source.filterMode = FilterMode.Bilinear;

        Graphics.Blit(source, destination, blurMaterial);
    }

}
