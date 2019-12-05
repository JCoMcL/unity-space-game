Shader "Custom/Holographic"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Col1 ("Color 1",color) = (1,1,1,1)
		_Col2 ("Color 2",color) = (0,0,0,1)
		_Speed ("Scroll Speed", float) = 0
		_Size ("Line Width", Range(0.0001,100)) = 1
		_Sharpness("Sharpness",Range(1,10)) = 1
		[Toggle] _Sharp ("Binary Tint", float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Blend One One
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float3 _Col1;
			float3 _Col2;
			float _Size;
			float _Speed;
			float _Sharpness;
			bool _Sharp;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float3 tint;
				float factor = sin(((i.vertex.y / _Size) + (_Time[0] * _Speed))*3.14159265359);
				if(_Sharp > 0)
				{
					if(factor < 0)	{tint = _Col1;}
					else 			{tint = _Col2;}
				}
				else
				{
					factor *= _Sharpness;
					factor = factor/2+0.5;
					factor = clamp(factor,0,1);
					tint = _Col1 * factor + _Col2 * (1-factor);
				}
				float4 tint4 = {tint[0],tint[1],tint[2],1};
				fixed4 col = tex2D(_MainTex, i.uv) * tint4;
				return col;
			}
			ENDCG
		}
	}
}
