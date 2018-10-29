Shader "Custom/Lab/ShowInside"
{
	properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("MainTex", 2D) = "White" {}
	}

	SubShader
	{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 200
		

		Pass
		{
			ZWrite On
			ColorMask 0
		}

		UsePass "Transparent/Diffuse/FOWARD"
	}
	Fallback "Transparent/VertexLit"
}
