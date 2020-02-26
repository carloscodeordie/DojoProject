using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
public class TVDistortion : UnityStandardAssets.ImageEffects.PostEffectsBase {

	[Range(0.0f, 1.0f)]
	public float distortionStrength = 0.5f;

	[Range(0.0f, 1.0f)]
	public float fisheyeStrength = 0.5f;

	[Range(0.0f, 1.0f)]
	public float stripesStrength = 0.5f;

	[Range(0.0f, 1.0f)]
	public float noiseStrength = 0.5f;

	[Range(0.0f, 1.0f)]
	public float vignetteStrength = 0.5f;

	public bool VHSScanlines;
	private float yScanline, xScanline;

	public Texture2D Noise;

	public Shader tvDistortionShader = null;
	private Material tvDistortionMaterial = null;

	public override bool CheckResources()
	{
		CheckSupport(false);

		tvDistortionMaterial = CheckShaderAndCreateMaterial(tvDistortionShader, tvDistortionMaterial);
		tvDistortionMaterial.SetTexture("_NoiseTex", Noise);

		if (!isSupported)
			ReportAutoDisable();
		return isSupported;
	}

	public void OnDisable()
	{
		if (tvDistortionMaterial)
			DestroyImmediate (tvDistortionMaterial);
	}

	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (CheckResources() == false)
		{
			Graphics.Blit(source, destination);
			return;
		}

		yScanline += Time.deltaTime * 0.1f;
		xScanline -= Time.deltaTime * 0.1f;

		if(yScanline >= 1){
			yScanline = Random.value;
		}
		if(xScanline <= 0 || Random.value < 0.05){
			xScanline = Random.value;
		}

		tvDistortionMaterial.SetFloat("_distortionStrength", distortionStrength);
		tvDistortionMaterial.SetFloat("_fisheyeStrength", fisheyeStrength);
		tvDistortionMaterial.SetFloat("_stripesStrength", stripesStrength);
		tvDistortionMaterial.SetFloat("_noiseStrength", noiseStrength);
		tvDistortionMaterial.SetFloat("_vignetteStrength", vignetteStrength);
		tvDistortionMaterial.SetFloat("_yScanline", yScanline);
		tvDistortionMaterial.SetFloat("_xScanline", xScanline);

		if(VHSScanlines)
			tvDistortionMaterial.SetInt("_vhsScanlines", 1);
		else
			tvDistortionMaterial.SetInt("_vhsScanlines", 0);

		source.filterMode = FilterMode.Bilinear;

		Graphics.Blit(source, destination, tvDistortionMaterial);
	}
}
