// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Stencil/Hole"
{
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue" = "Geometry-1" }
		
		ColorMask 0
		ZWrite off
	
		Stencil
		{
			Ref 1
			Comp always
			Pass replace
		}

		Pass
		{
			//ZTest Less

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{

				fixed4 col = fixed4(1,1,0,1);
				return col;
			}
			ENDCG
		}
	}
}
