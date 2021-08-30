Shader "Custom/VertexNormals"
{
    Properties
    {
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Normal("Normal", 2D) = "bump" {}

		_Color("Color", Color) = (1,1,1,1)

		_Value("Value", Range(0,1)) = 1.0

		_ToonValue("ToonValue", Range(0,5)) = 0.35
		_VertexNormals("VertexNormals", Range(0, 10)) = 1.0

		_RimDarkColor("Rim Dark Color", Color) = (0.5, 0.5, 0.4, 1.0)
		_RimLightColor("Rim Light Color", Color) = (1.0, 1.0, 0.5, 1.0)
		_RimPower("Rim Power", Range(0.1, 3.0)) = 1.1
		_RimSoft("Rim Soft", Range(0, 1)) = 0.9

		_EdgeColor("Edge Color", Color) = (0,0,0,1)
		_EdgeWidth("Edge width", Range(.000, 0.1)) = .005
		_EdgePower("Edge Power", Range(0.0, 1.0)) = 0.0
	}
	SubShader
	{
		Tags {"RenderType" = "Transparent" "Queue" = "Geometry"}
		LOD 200
		Pass {
			ColorMask 0
		}
		Blend SrcAlpha OneMinusSrcAlpha 

		CGPROGRAM
		#pragma surface surf Toon //alpha noforwardadd halfasview approxview noambient
		float _VertexNormals;
		float _ToonValue;

		float4 _RimDarkColor;
		float4 _RimLightColor;
		float _RimPower;
		float _RimSoft;

		fixed4 LightingToon(SurfaceOutput s, fixed3 lightDir, fixed atten) {
			//Calculate Diffuse Term
			fixed dir = abs(length(lightDir - s.Normal));
			if (dir < _VertexNormals)
				s.Normal = (lightDir);
			else
				s.Normal = -(lightDir * dir);
			fixed NL = dot(s.Normal, lightDir);
			fixed diff = 0.2 + max(NL, 0);
			fixed3 diffColor = s.Albedo * _LightColor0.rgb * (diff * (atten * 2));
			//Sum up
			fixed4 c;
			if (diff < 0.3) c.rgb = diffColor * 0.3 * _ToonValue;		// Low shades multiplier
			else if (diff < 0.8) c.rgb = diffColor * 0.4 * _ToonValue;	//Medium shades multiplier
			else c.rgb = diffColor * 0.5 * _ToonValue;					// Light shades multiplier
			c.a = s.Alpha;
			return c;
		}

		sampler2D _MainTex;
		fixed4 _Color;
		float _Value;

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;

		struct Input
		{
			float2 uv_MainTex;
			float3 viewDir;
			float3 lightDir;
		};

		void vert(inout appdata_full v, out Input o) {
			o.lightDir = WorldSpaceLightDir(v.vertex);
		}

		void surf(Input IN, inout SurfaceOutput o)
		{
			float2 uv_Normal = IN.uv_MainTex * _Normal_ST.xy + _Normal_ST.zw;
			o.Normal = UnpackNormal(tex2D(_Normal, uv_Normal));

			_Color.a = _Value;
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			
			half rim = _RimPower - saturate(dot(normalize(IN.viewDir), o.Normal));
			
			float4 rimColor;
			
			if (saturate(dot(normalize(IN.viewDir), o.Normal)) <= 0)
				rimColor = _RimDarkColor;
			else
				rimColor = _RimLightColor;

			if (rim < _RimSoft) rim = 0;

			o.Emission = c.rgb * rimColor.rgb * rim;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG



		//외곽선
		Pass
		{
			Cull Front
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f {
				float4 pos : SV_POSITION;
				float4 color : COLOR;
			};
			fixed4 _EdgeColor;
			fixed _EdgeWidth;
			fixed _EdgePower;
			float _Value;

			float4 vert(float4 position : POSITION,
						float3 normal : NORMAL) : SV_POSITION {
				float4 clipPosition = UnityObjectToClipPos(position);
				float3 clipNormal = mul((float3x3) UNITY_MATRIX_VP, mul((float3x3) UNITY_MATRIX_M, normal));
				float2 offset = normalize(clipNormal.xy) * _EdgeWidth * clipPosition.w;
				clipPosition.xy += offset;
				return clipPosition;
			}

			half4 frag(v2f i) : COLOR {
				if (_Value <= 0)
					_EdgeColor.a = 0;
				else
					_EdgeColor.a = 1;
				return _EdgeColor + _EdgePower;
			}

			ENDCG
		}
	}
	FallBack "Diffuse"
}