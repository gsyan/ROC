// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Red"
{
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Geometry"}
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float4 tangent : TANGENT;
			};
            struct v2f {
                float4 pos : SV_POSITION;
				fixed4 color : COLOR;
            };
            v2f vert(appdata v) {
                v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				//o.color = v.tangent * 0.5 + 0.5;
				o.color = 0;
				
				/*o.pos = UnityObjectToClipPos(v.vertex);*/
                return o;
            }
            half4 frag(v2f i) : SV_Target {
				half4 c = i.color;
				/*c.r = 0;
				c.g = 0.5;
				c.b = 0.5;*/

				return c;
            }
            ENDCG
        }
	}
}