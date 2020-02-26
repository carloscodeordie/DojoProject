using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
public class CRT : UnityStandardAssets.ImageEffects.PostEffectsBase {

	[Range(0.0f, 10.0f)]
	public float PixelSize = 3.0f;

	[Range(0.0f, 1.0f)]
	public float CRTFade = 0.0f;

	[Range(0.0f, 1.0f)]
	public float Brightness = 0.0f;

	public Shader crtShader = null;
	private Material crtMaterial = null;

	public override bool CheckResources()
	{
		CheckSupport(false);

		crtMaterial = CheckShaderAndCreateMaterial(crtShader, crtMaterial);

		if (!isSupported)
			ReportAutoDisable();
		return isSupported;
	}

	public void OnDisable()
	{
		if (crtMaterial)
			DestroyImmediate(crtMaterial);
	}

	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (CheckResources() == false)
		{
			Graphics.Blit(source, destination);
			return;
		}

		crtMaterial.SetFloat ("_PixelSize", PixelSize);
		crtMaterial.SetFloat ("_CRTFade", CRTFade);
		crtMaterial.SetFloat ("_Brightness", Brightness);
		source.filterMode = FilterMode.Bilinear;

		Graphics.Blit(source, destination, crtMaterial);
	}
}
