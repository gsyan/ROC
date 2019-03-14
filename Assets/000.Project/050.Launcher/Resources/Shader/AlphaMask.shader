// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Threepoint/AlphaMask"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_AlphaTex ("Alpha mask (R)", 2D) = "white" {}
	}

	CGINCLUDE

	#pragma vertex vert
	#pragma fragment frag
	
	#include "UnityCG.cginc"
	
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
	
	sampler2D _MainTex;
	sampler2D _AlphaTex;
	
	float4 _MainTex_ST;
	
	v2f vert (appdata_t v)
	{
	    v2f o;

	    o.pos = UnityObjectToClipPos(v.pos);
	    o.uv0 = TRANSFORM_TEX(v.uv0, _MainTex);
		o.uv1 = v.uv1;

	    return o;
	}
	
	fixed4 frag (v2f i) : SV_Target
	{
	    fixed4 output = tex2D(_MainTex, i.uv0);
	    output.a = tex2D(_AlphaTex, i.uv1).r;

	    return output;
	}

	ENDCG
	
	SubShader
	{
		Tags
		{	
			"RenderType"="Transparent"
			"Queue"="Transparent"
		}
		
		LOD 100
		
		Cull Off
		Lighting Off
		ZWrite Off
		Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha 
		
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