// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Test4"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			// note: no SV_POSITION in this struct
			struct v2f
			{
				float2 uv : TEXCOORD0;
			};

			v2f vert(
				float4 vertex : POSITION, // vertex position input
				float2 uv : TEXCOORD0, // texture coordinate input
				out float4 outpos : SV_POSITION // clip space position output
			)
			{
				v2f o;
				o.uv = uv;
				outpos = UnityObjectToClipPos(vertex);
				return o;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f i, UNITY_VPOS_TYPE screenPos : VPOS) : SV_Target
			{
				screenPos.xy = floor(screenPos.xy * 0.25) * 0.5;
				float checker = -frac(screenPos.x + screenPos.y);

				clip(checker);

				fixed4 c = tex2D(_MainTex, i.uv);
				return c;
			}
			ENDCG
		}
	}
}