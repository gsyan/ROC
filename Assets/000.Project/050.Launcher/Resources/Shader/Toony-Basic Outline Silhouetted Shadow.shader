// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Threepoint/Toony-Basic Outline Silhouetted Shadow"
{
	Properties
	{
		_Color("Main Color", Color) = (.5, .5, .5, 1)
		_OutlineColor("Outline Color", Color) = (0.0, 0.0, 0.0, 1.0)
		_Outline("Outline Width", Range(0.0, 0.05)) = 0.02
		_SilhouetteColor("Silhouette Color", Color) = (0.0, 0.0, 0.0, 1.0)
		_Silhouette("Silhouette Width", Range(0.0, 0.1)) = 0.05
		_MainTex("Base (RGB)", 2D) = "white" { }
		_ToonShade("ToonShader Cubemap(RGB)", CUBE) = "" { }
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	struct v2f
	{
		float4 pos : POSITION;
		float4 color : COLOR;
	};

	uniform float _Outline;
	uniform float4 _OutlineColor;
	uniform float _Silhouette;
	uniform float4 _SilhouetteColor;

	v2f vert(appdata v)
	{
		// just make a copy of incoming vertex data but scaled according to normal direction
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);

		float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
		float2 offset = TransformViewToProjection(norm.xy);

		o.pos.xy += offset * _Silhouette;
		o.color = _SilhouetteColor;
		return o;
	}
	ENDCG

	SubShader
	{
		Tags{ "Queue" = "Transparent" }

		// note that a vertex shader is specified here but its using the one above
		Pass
		{
			Name "OUTLINE"
			Tags{ "LightMode" = "Always" }
			Cull Off
			ZWrite Off
			//ZTest Always
			ColorMask RGB // alpha not used

			// you can choose what kind of blending mode you want for the outline
			Blend SrcAlpha OneMinusSrcAlpha // Normal
			//Blend One One // Additive
			//Blend One OneMinusDstColor // Soft Additive
			//Blend DstColor Zero // Multiplicative
			//Blend DstColor SrcColor // 2x Multiplicative

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			half4 frag(v2f i) :COLOR
			{
				return i.color;
			}
			ENDCG
		}

		UsePass "Threepoint/Toony-Basic/BASE"
		UsePass "Threepoint/Outline/BASE"
		UsePass "Threepoint/FlatShadow/BASE"
	}
	
	Fallback "Diffuse"
}