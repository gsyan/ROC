// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Test5"
{
	Properties
	{
		_ColorFront("FrontColor", Color) = (1,0.7,0.7,1)
		_ColorBack("BackColor", Color) = (0.7,1,0.7,1)
	}
	SubShader
	{
		Pass
		{
			Cull Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			float4 vert(float4 vertex : POSITION) : SV_POSITION
			{
				return UnityObjectToClipPos(vertex);
			}

			fixed4 _ColorFront;
			fixed4 _ColorBack;

			fixed4 frag( fixed facing : VFACE ) : SV_Target
			{
				return facing > 0 ? _ColorFront : _ColorBack;
			}
			ENDCG
		}
	}
}