// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Threepoint/Particles/Additive"
{
	Properties
	{
		_MainTex("Particle Texture", 2D) = "white" {}
		_TintColor("TintColor", Color) = (0.5, 0.5, 0.5, 0.5)
	}

	Category
	{
		SubShader
		{
			Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
			ZWrite Off
			Cull Off
			Blend SrcAlpha One
			Fog { Mode Off }

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog

				#include "UnityCG.cginc"

				sampler2D _MainTex;

				struct appdata_t {
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord : TEXCOORD0;
					//UNITY_FOG_COORDS(1)
				};

				float4 _MainTex_ST;
				fixed4 _TintColor;

				v2f vert(appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.color = v.color;
					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					//UNITY_TRANSFER_FOG(o, o.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = 2.0 * _TintColor * i.color * tex2D(_MainTex, i.texcoord);
					//UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0, 0, 0, 0)); // fog towards black due to our blend mode
					return col;
				}
				ENDCG
			}
		}
	}
}