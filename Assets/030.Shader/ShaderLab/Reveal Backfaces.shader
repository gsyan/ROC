Shader "Custom/Lab/Reveal Backfaces"
{
	properties
	{
		_MainTex("MainTex", 2D) = "White" {}
	}

	SubShader
	{
		Pass
		{
			Material
			{
				Diffuse(1,1,1,1)
			}
			Lighting On
			SetTexture[_MainTex]
			{
				Combine Primary * Texture
			}
		}

		Pass
		{
			Color(1,0,1,1)
			Cull Front
		}
	}
}
