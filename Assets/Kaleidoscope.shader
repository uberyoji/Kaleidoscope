// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Kaleidoscope"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Pivot("Pivot", Vector) = (0.5,0.5,0,0)
		_Angle("Angle", Range(-5.0,  5.0)) = 0.0
		_Scroll("Scroll", Range(0.0,  1.0)) = 0.0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"

				struct v2f {
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
				};

				float _Angle;
				float _Scroll;
				float4 _Pivot;
				float4 _MainTex_ST;

				v2f vert(appdata_base v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);

					// Pivot
					// float2 pivot = float2(0.5, 0.5);
					// Rotation Matrix
					float cosAngle = cos(_Angle);
					float sinAngle = sin(_Angle);
					float2x2 rot = float2x2(cosAngle, -sinAngle, sinAngle, cosAngle);

					// Rotation consedering pivot
					float2 uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
					
					uv.y += _Scroll;
					uv -= _Pivot.xy;
					uv.y -= _Scroll;
					o.uv = mul(rot, uv);
					o.uv.y += _Scroll;
					o.uv += _Pivot.xy;

					return o;
				}

				sampler2D _MainTex;

				fixed4 frag(v2f i) : SV_Target
				{
					// Texel sampling
					return tex2D(_MainTex, i.uv);
				}

				ENDCG
			}
		}
}