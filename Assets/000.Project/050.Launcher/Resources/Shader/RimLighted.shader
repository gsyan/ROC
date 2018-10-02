// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Threepoint/RimLighted"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_MainColor("Main Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_RimColor("Rim Color", Color) = (1.0, 1.0, 1.0, 1.0)
      	_RimPower("Rim Power", Range(0.5, 8.0)) = 3.0
      	_RimWidth("Rim Width", Range(0.0, 1.0)) = 0.8
	}
	
	SubShader
	{	
		Pass
		{
			Name "BASE"

			Tags { "RenderType"="Opaque" }

			LOD 200
			Lighting Off
			Blend Off
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			sampler2D _MainTex;
			float4 _MainColor;
			float4 _RimColor;
			float _RimPower;
			float _RimWidth;
			
			struct vertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 texcoord : TEXCOORD0;
			};
			
			struct vertexOutput
			{
				float4 pos : SV_POSITION;
				float2 tex : TEXCOORD0;
				float3 posWorld :TEXCOORD1;
				float3 normalDir :TEXCOORD2;
			};			
			
			vertexOutput vert(vertexInput v)
			{
				vertexOutput o;
				o.posWorld = mul(unity_ObjectToWorld, v.vertex);
				o.normalDir = normalize(mul(float4(v.normal, 0.0f), unity_WorldToObject).xyz);
				o.pos = UnityObjectToClipPos(v.vertex);
				o.tex = v.texcoord;
				
				return o;
			}
			
			float4 frag(vertexOutput i) : COLOR
			{
				float3 normalDirection = i.normalDir;
				float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
				
				float4 c = tex2D(_MainTex, i.tex) * _MainColor;
				float rim = smoothstep(1.0f - _RimWidth, 1.0f, 1.0f - max(0.0f, dot(normalize(viewDirection), i.normalDir)));
				
				return c + float4(_RimColor.rgb * pow(rim, _RimPower), 1.0f);
			}
			ENDCG
		}
	}
}

