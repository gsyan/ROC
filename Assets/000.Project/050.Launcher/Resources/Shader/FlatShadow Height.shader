// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Threepoint/FlatShadow Height"
{
	SubShader
	{
		Tags{ "RenderType" = "Transparent" }

		Pass
		{
			Name "BASE"
			
			Stencil
			{
				Ref 0
				Comp Equal
				Pass IncrWrap
				ZFail Keep
			}

			Lighting Off
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			float4 _LightDir;
			float4 _ShadowColor;

			struct appdata
			{
				float4 vertex : POSITION;
			};
			
			struct v2f
			{
				float4 pos : SV_POSITION;
			};
			
			v2f vert(appdata v)
			{
				v2f o;
				
				float4 vpos = mul(unity_ObjectToWorld, v.vertex);
				float t = (vpos.y - unity_ObjectToWorld._24) / dot(float3(0.0f, -1.0f, 0.0f), _LightDir);

				vpos.xyz = vpos.xyz + _LightDir * t;
				o.pos = mul(UNITY_MATRIX_VP, vpos);
				
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				return fixed4(_ShadowColor);
			}
			ENDCG
		}
	} 
}

