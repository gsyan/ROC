Shader "Custom/Diffuse Emissive"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Power ("Power", Range (0.0, 10.0)) = 5.0
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		uniform float _Power;

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o)
		{
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb + c.rgb * c.a * _Power;
		}
		ENDCG
	}
	
	FallBack "Diffuse"
}
