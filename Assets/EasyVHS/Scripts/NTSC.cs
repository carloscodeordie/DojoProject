using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
public class NTSC : UnityStandardAssets.ImageEffects.PostEffectsBase {

	[Range(0.0f, 10.0f)]
	public float YFrequency = 6.0f;

	[Range(0.0f, 10.0f)]
	public float IFrequency = 1.2f;

	[Range(0.0f, 10.0f)]
	public float QFrequency = 0.6f;

	public Shader ntscShader = null;
	private Material ntscMaterial = null;

	public override bool CheckResources()
	{
		CheckSupport(false);

		ntscMaterial = CheckShaderAndCreateMaterial(ntscShader, ntscMaterial);

		if (!isSupported)
			ReportAutoDisable();
		return isSupported;
	}

	public void OnDisable()
	{
		if (ntscMaterial)
			DestroyImmediate(ntscMaterial);
	}

	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (CheckResources() == false)
		{
			Graphics.Blit(source, destination);
			return;
		}

		ntscMaterial.SetFloat ("_YFrequency", YFrequency);
		ntscMaterial.SetFloat ("_IFrequency", IFrequency);
		ntscMaterial.SetFloat ("_QFrequency", QFrequency);
		source.filterMode = FilterMode.Bilinear;

		Graphics.Blit(source, destination, ntscMaterial);
	}
}
