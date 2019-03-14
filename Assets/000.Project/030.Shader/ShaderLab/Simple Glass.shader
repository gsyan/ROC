//오브젝트 뒷면을 완전 무시한다.

Shader "Custom/Lab/Simple Glass"
{
	properties
	{
		_Color("Main Color", Color) = (1,1,1,0)
		/*_SpecColor("Spec Color", Color) = (1,1,1,1)
		_Emission("Emmisive Color", Color) = (0,0,0,0)
		_Shininess("Shininess", Range(0.01, 1)) = 0.7*/
		_MainTex("MainTex", 2D) = "White" {}
	}

	SubShader
	{
		Material
		{
			Diffuse[_Color]
			/*Ambient[_Color]
			Shininess[_Shininess]
			Specular[_SpecColor]
			Emission[_Emission]*/
		}
		lighting on
		SeparateSpecular On
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			cull front
			settexture[_MainTex]
			{
				combine primary * texture
			}
		}

		Pass
		{
			cull back
			settexture[_MainTex]
			{
				combine primary * texture
			}
		}
	}
}
