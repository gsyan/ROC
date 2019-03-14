// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Threepoint/Additive AlphaMask"
{
	Properties
	{
		_MainTex ("Base", 2D) = "white" {}
		_AlphaTex ("Alpha mask (R)", 2D) = "white" {}
		_TintColor ("TintColor", Color) = (1.0, 1.0, 1.0, 1.0)
	}
	
	CGINCLUDE

	#include "UnityCG.cginc"

	sampler2D _MainTex;
	sampler2D _AlphaTex;
	fixed4 _TintColor;
	
	half4 _MainTex_ST;
					
	struct appdata_t
	{
	    float4 pos : POSITION;
	    float2 uv0 : TEXCOORD0;
		float2 uv1 : TEXCOORD1;
	};
	
	struct v2f
	{
	    float4 pos : SV_POSITION;
	    half2 uv0 : TEXCOORD0;
	    half2 uv1 : TEXCOORD1;
	};

	v2f vert(appdata_t v)
	{
		v2f o;
		
		o.pos = UnityObjectToClipPos (v.pos);	
		o.uv0 = TRANSFORM_TEX(v.uv0, _MainTex);
		o.uv1 = v.uv1;
				
		return o;
	}
	
	fixed4 frag( v2f i ) : COLOR
	{	
		fixed4 output = tex2D(_MainTex, i.uv0) * _TintColor;
		output.a = tex2D(_AlphaTex, i.uv1).r;

		return output;
	}
	
	ENDCG
	
	SubShader
	{
		Tags
		{
			"RenderType" = "Transparent"
			"Reflection" = "RenderReflectionTransparentAdd"
			"Queue"="Transparent"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend SrcAlpha One
		
		Pass
		{
			CGPROGRAM
		
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest 
		
			ENDCG
		}
	} 

	FallBack Off
}
