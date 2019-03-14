Shader "Custom/Lab/Alpha Blended Textures"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		//_BlendTex have to be a alpha channel
		_BlendTex("Alpha Blended (RGBA) ", 2D) = "white" {}
	}
	SubShader
	{
		Pass
		{
			// Apply base texture
			SetTexture[_MainTex]
			{
				combine texture
			}
			// Blend in the alpha texture using the lerp operator
			SetTexture[_BlendTex]
			{
				combine texture lerp(texture) previous
			}
		}
	}
}
