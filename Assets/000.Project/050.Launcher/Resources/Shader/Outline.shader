// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Threepoint/Outline"
{
	Properties
	{
		_OutlineColor("Outline Color", Color) = (0.0, 0.0, 0.0, 1.0)
		_Outline("Outline Width", Range(0.0, 0.05)) = 0.02
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
	
	v2f vert(appdata v)
	{
		// just make a copy of incoming vertex data but scaled according to normal direction
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);

		float3 norm = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
		float2 offset = TransformViewToProjection(norm.xy);

		o.pos.xy += offset * _Outline;
		o.color = _OutlineColor;
		return o;
	}
	ENDCG

	SubShader
	{
		Tags { "RenderType" = "Opaque" }

		Pass
		{
			Name "BASE"

			Cull front
			ZWrite on
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			half4 frag(v2f i) :COLOR
			{
				return i.color;
			}
			ENDCG
		}
	}
	
	Fallback "Toon/Basic"
}
