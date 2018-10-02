Shader "Threepoint/Toony-Basic Shadow"
{
	Properties
	{
		_Color ("Main Color", Color) = (.5,.5,.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ToonShade ("ToonShader Cubemap(RGB)", CUBE) = "" { }
	}


	SubShader
	{
		UsePass "Threepoint/Toony-Basic/BASE"
		UsePass "Threepoint/FlatShadow/BASE"
	}
	
	Fallback "Unlit/Texture"
}
