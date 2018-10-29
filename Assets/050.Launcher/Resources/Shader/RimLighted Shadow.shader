Shader "Threepoint/RimLighted Shadow"
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
		UsePass "Threepoint/RimLighted/BASE"
		UsePass "Threepoint/FlatShadow/BASE"
	} 
	
	Fallback "Unlit/Texture"
}
