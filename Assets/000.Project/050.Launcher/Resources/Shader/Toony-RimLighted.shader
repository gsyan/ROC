Shader "Threepoint/Toony-RimLighted"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Ramp("Toon Ramp (RGB)", 2D) = "gray" {}
		_Emissive("Emissive (RGB)", 2D) = "black" {}
		_MainColor("Main Color", Color) = (0.5, 0.5, 0.5, 1.0)
		_RimColor("Rim Color", Color) = (1.0, 1.0, 1.0, 1.0)
      	_RimPower("Rim Power", Range(0.5, 8.0)) = 3.0
      	_RimWidth("Rim Width", Range(0.0, 1.0)) = 0.8
	}

	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
		}
		
		CGPROGRAM
		#pragma surface surf ToonRamp
		
		sampler2D _MainTex;
		sampler2D _Ramp;		
		sampler2D _Emissive;
		float4 _MainColor;
		float4 _RimColor;
		float _RimPower;
		float _RimWidth;
		
		#pragma lighting ToonRamp exclude_path:prepass
		inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten)
		{
			#ifndef USING_DIRECTIONAL_LIGHT
			lightDir = normalize(lightDir);
			#endif
			
			half d = dot (s.Normal, lightDir) * 0.5 + 0.5;
			half3 ramp = tex2D (_Ramp, float2(d,d)).rgb;
			
			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
			c.a = 0;
			return c;
		}
		
		struct Input
		{
			float2 uv_MainTex;
			float3 viewDir;
		};
		
		void surf (Input IN, inout SurfaceOutput o)
		{
			half4 c = tex2D(_MainTex, IN.uv_MainTex) * _MainColor;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			
			half rim = smoothstep(1.0 - _RimWidth, 1.0, 1.0 - max(0.0, dot(normalize(IN.viewDir), o.Normal)));
         	o.Emission = _RimColor.rgb * pow (rim, _RimPower);
         	
         	half4 emi = tex2D(_Emissive, IN.uv_MainTex);
         	o.Emission += emi.rgb * emi.a;
		}
		ENDCG
	} 

	Fallback "Toon/Lighted"
}
