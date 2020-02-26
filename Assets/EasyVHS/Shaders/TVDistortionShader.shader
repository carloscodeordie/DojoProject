// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/TVDistortionShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{

			Name "TVDISTORTION"

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _NoiseTex;

			int _vhsScanlines;
			float _distortionStrength;
			float _fisheyeStrength;
			float _stripesStrength;
			float _noiseStrength;
			float _vignetteStrength;
			float _yScanline;
			float _xScanline;

			float noise(float2 p)
			{
				float2 ss = 65.0*float2(1.0, 2.0*cos(4.0*_Time.y)) ;
				float sample = tex2D(_NoiseTex, 65.*float2(1., 2.*cos(4.*_Time.y)) + float2(p.x, p.y*.25)).x;
				sample *= sample;
				return sample;
			}

			float onOff(float a, float b, float c)
			{
				//Scale time for better looking distortion
				float time = _Time * 16;
				return step(c, sin(time + a*cos(time*b)));
			}

			float ramp(float y, float start, float end)
			{
				float inside = step(start,y) - step(end,y);
				float fact = (y-start)/(end-start)*inside;
				return (1.-fact) * inside;
			}

			float stripes(float2 uv)
			{
				//Scale time for faster stripes
				float time = _Time * 24;
				float noi = noise(uv*float2(0.5,1.) + float2(1.,3.));
				return ramp(fmod(uv.y*4. + time/2.+sin(time + sin(time*0.63)),1.),0.5,0.6)*noi;
			}

			float3 getVideo(float2 uv)
			{
				float2 olduv = uv;

				//Scale time for better looking distortion
				float time = _Time * 16;
				float2 look = uv;
				float window = 1./(1.+20.*(look.y-fmod(time/4.,1.))*(look.y-fmod(time/4.,1.)));
				look.x = look.x + sin(look.y*10. + time)/50.*onOff(4.,4.,.3)*(1.+cos(time*80.))*window;
				float vShift = onOff(2.,3.,.9)*(sin(time)*sin(time*20.) + 
													 (0.5 + 0.1*sin(time*200.)*cos(time)));
				look.y = fmod(look.y + vShift, 1.);

				look = lerp(olduv, look, _distortionStrength);

				float3 video = float3(tex2D(_MainTex, look).xyz);

				return video;
			}

			float2 screenDistort(float2 uv)
			{
				float2 newuv = uv;
				newuv -= float2(.5,.5);
				newuv = newuv*1.2*(1./1.2+2.*newuv.x*newuv.x*newuv.y*newuv.y);
				newuv += float2(.5,.5);

				newuv = lerp(uv, newuv, _fisheyeStrength);

				return newuv;
			}

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{

				float2 uv = i.uv;
				//Apply TV distortion

				//VHS scanlines
				if(_vhsScanlines==1)
				{
					float dx = 1-abs(distance(uv.y, _xScanline));
					float dy = 1-abs(distance(uv.y, _yScanline));

					uv.x += dy * 0.02;

					uv.y += step(0.99, dx) * dx;

					if(dx > 0.99)
					uv.y = _xScanline;
					uv.y = step(0.99, dy) * (_yScanline) + step(dy, 0.99) * uv.y;
				}

				//TV screen distortion
				uv = screenDistort(uv);
				float3 video = getVideo(uv);
				float vigAmt = _vignetteStrength*(3.+.3*sin(_Time + 5.*cos(_Time*5.)));
				float vignette = (1.-vigAmt*(uv.y-.5)*(uv.y-.5))*(1.-vigAmt*(uv.x-.5)*(uv.x-.5));
				video += _stripesStrength*stripes(uv);
				video += _noiseStrength*noise(uv*2.)/2.;
				video *= vignette;
				video *= (12.+fmod(uv.y*30.+_Time,1.))/13.;

				fixed4 col=fixed4(video,1.0);

				return col;
			}

			ENDCG
		}
	}
}
