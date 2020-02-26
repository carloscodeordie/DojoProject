// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/BlurShader"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}

	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{

			Name "BLUR"
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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;

			float _amount;
			float _channelDif;
			float _width;
			float _height;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

				// blur amount transformed into texel space
				float blurH = _amount / _height;
				float blurV = _amount / _width;

				// Kernel
				fixed2 offsets[8] = 
				{
					fixed2(blurH, 0),
					fixed2(-blurH, 0),
					fixed2(0, blurV),
					fixed2(0, -blurV),
					fixed2(blurH, blurV),
					fixed2(blurH, -blurV),
					fixed2(-blurH, blurV),
					fixed2(-blurH, -blurV),
				};

				fixed4 samples[8];
				fixed4 samplesRed[8];
				for (int ii = 0; ii < 8; ii++)
				{
					samples[ii] = tex2D(_MainTex, i.uv + offsets[ii]);
					samplesRed[ii] = tex2D(_MainTex, i.uv + offsets[ii]+fixed2(_channelDif/_width, 0.0));

					col.r += samplesRed[ii].r;
					col.gb += samples[ii].gb;
				}

				col /= 9.0;

				return col;
			}

			ENDCG


		}


	}
	Fallback off
}
