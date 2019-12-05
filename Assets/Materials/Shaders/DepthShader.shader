// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/FakeDepth"
{
	Properties
	{
		_Tint ("Tint", Color) = (1,1,1,1)
		_Tex1 ("Main Texture", 2D) = "black" {}
		_Dist1("Main Texture Size", Range (0, 1)) = 0.5
		_Tex2 ("Overlay Texture 0", 2D) = "black" {}
		_Dist2("Overlay Texture Size 0", Range (0, 1)) = 0.5		
		_Tex3 ("Overlay Texture 1", 2D) = "black" {}
		_Dist3("Overlay Texture Size 0", Range (0, 1)) = 0.5

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 pos : TEXCOORD1;
			};

			sampler2D _Tex1;
			sampler2D _Tex2;
			sampler2D _Tex3;
			float4 _Tint;
			float4 _Tex1_ST;
			float _Dist1;
			float _Dist2;
			float _Dist3;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = mul(unity_ObjectToWorld, v.vertex)/100;	
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _Tex1);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_Tex1, i.pos.xy * (1 -_Dist1) + i.uv * _Dist1);
				col += tex2D(_Tex2, i.pos.xy * (1 -_Dist2) + i.uv * _Dist2);
				col += tex2D(_Tex3, i.pos.xy * (1 -_Dist3) + i.uv * _Dist3);
				col *= _Tint;
				return col;
			}
			ENDCG
		}
	}
}
