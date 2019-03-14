Shader "Threepoint/Terrain"
{
	Properties
	{
		_Tex1 ("Base (RGB)", 2D) = "white" {}
		_Tex2 ("Sub (RGB)", 2D) = "white" {}
		_MaskTex ("Mask (RGB)", 2D) = "white" {}
		_Color ("Main Color", Color) = (1.0,1.0,1.0,1.0)
    }
    
	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
		}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert
		
		sampler2D _Tex1;
		sampler2D _Tex2;
		sampler2D _MaskTex;
		fixed4 _Color;
		
		struct Input
		{
			float2 uv_Tex1;
			float2 uv_Tex2;
			float2 uv_MaskTex;
		};
		
		void surf (Input IN, inout SurfaceOutput o)
		{
			float4 tex1 = tex2D (_Tex1, IN.uv_Tex1);
			float4 tex2 = tex2D (_Tex2, IN.uv_Tex2);
			float alpha = tex2D (_MaskTex, IN.uv_MaskTex).r;
			
			o.Albedo = lerp(tex2, tex1, alpha) * _Color;
		}
		ENDCG
	}
	
	FallBack "Diffuse"
}

