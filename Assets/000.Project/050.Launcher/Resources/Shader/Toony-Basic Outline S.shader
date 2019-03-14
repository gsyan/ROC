Shader "Threepoint/Toony-Basic Outline S"
{
	Properties
	{
		_Color("Main Color", Color) = (.5, .5, .5, 1)
		_OutlineColor("Outline Color", Color) = (0.0, 0.0, 0.0, 1.0)
		_Outline("Outline Width", Range(0.0, 0.05)) = 0.02
		_MainTex("Base (RGB)", 2D) = "white" { }
		_ToonShade("ToonShader Cubemap(RGB)", CUBE) = "" { }
	}


	SubShader
	{		
		UsePass "Threepoint/Toony-Basic/BASE"
		UsePass "Threepoint/Outline/BASE"
		UsePass "Threepoint/FlatShadow/BASE"
	} 
	
	Fallback "Unlit/Texture"
}
