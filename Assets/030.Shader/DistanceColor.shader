Shader "Custom/DistanceColor" {
	Properties{
		_TopLine("Top Line Y", Float) = 0
		_BottomLine("Bottom Line Y", Float) = 0
		_GradientColor1("GradientTopColor1", Color) = (1  ,0,0,1)
		_GradientColor2("GradientTopColor2", Color) = (0.8,0,0,1)
		_GradientColor3("GradientTopColor3", Color) = (0.6,0,0,1)
		_GradientColor4("GradientTopColor4", Color) = (0.3,0,0,1)
		_GradientColor5("GradientTopColor5", Color) = (0  ,1,0,1)
		_TargetObjectPosition("Target Object Pos", Vector) = (0,0,0,0)
		_ThisObjectPosition("This Object Pos", Vector) = (0,0,0,0)
	}

		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 100

		Pass{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

		struct appdata_t {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float4 color : COLOR;
	};

	struct v2f {
		float4 vertex : SV_POSITION;
		float3 worldPos : TEXCOORD0;
		float3 normal : NORMAL;
		float4 color : COLOR;
	};

	fixed _TopLine;
	fixed _BottomLine;
	fixed4 _GradientColor1;
	fixed4 _GradientColor2;
	fixed4 _GradientColor3;
	fixed4 _GradientColor4;
	fixed4 _GradientColor5;
	fixed4 _TargetObjectPosition;
	fixed4 _ThisObjectPosition;

	// remap value to 0-1 range
	float remap(float value, float minSource, float maxSource)
	{
		return (value - minSource) / (maxSource - minSource);
	}

	v2f vert(appdata_t v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
		float4 normal4 = { v.normal.x, v.normal.y, v.normal.z, 1 };
		o.normal = mul(unity_ObjectToWorld, normal4).xyz;
		o.color = v.color;
		return o;
	}

	fixed4 frag(v2f IN) : COLOR
	{
		fixed4 c = fixed4(0,0,0,0);
		
		//타겟 오브젝트의 방향 벡터
		fixed3 targetDirection = (_ThisObjectPosition - _ThisObjectPosition).xyz;
		targetDirection = normalize(targetDirection);
		//방향성이 맞지 않는 면은 패스
		fixed dotValue = dot(IN.normal, -targetDirection);
		if (dotValue < 0.0)
		{
			return c;
		}
		
		//내적 이용
		/*
		fixed3 targetPosition = _TargetObjectPosition.xyz;
		fixed3 direction = IN.worldPos - targetPosition;
		direction = normalize(direction);
		dotValue = dot(direction, targetDirection);
		dotValue = dotValue * 2 - 1;
		dotValue = 1 - dotValue;
		c.xyz = _GradientTopColor * dotValue;
		*/

		fixed distanceFromTarget = distance(_TargetObjectPosition.xyz, IN.worldPos);
		if (distanceFromTarget < 1)
		{
			c.xyz = _GradientColor1;
		}
		else if (distanceFromTarget < 2)
		{
			c.xyz = _GradientColor2;
		}
		else if (distanceFromTarget < 3)
		{
			c.xyz = _GradientColor3;
		}
		else if (distanceFromTarget < 4)
		{
			c.xyz = _GradientColor4;
		}
		else if (distanceFromTarget < 5)
		{
			c.xyz = _GradientColor5;
		}
		else
		{
			c = IN.color;
		}

		
		return c;
	}
		ENDCG
	}
	}
}