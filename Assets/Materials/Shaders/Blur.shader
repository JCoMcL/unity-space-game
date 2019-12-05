
Shader "Custom/Blur"
{
	Properties 
	{
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_Color ("Main Color", Color) = (1,0,0,1.5)
		_BlurAmount ("Blur Amount", Range(0,1)) = 0.0005
		_BlendQuality("Blend Quality", int) = 3
		_OffsetQuality("Offset Quality", int) = 4
	}
	
	Category 
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		ZWrite Off
		//Alphatest Greater 0
		Blend SrcAlpha OneMinusSrcAlpha 
		Fog { Color(0,0,0,0) }
		Lighting Off
		Cull Off //we can turn backface culling off because we know nothing will be facing backwards

		BindChannels 
		{
			Bind "Vertex", vertex
			Bind "texcoord", texcoord 
			Bind "Color", color 
		}

		SubShader   
		{
			Pass 
			{
				//SetTexture [_MainTex] 
				//{
				//	Combine texture * primary
				//}
				
				
				
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
#pragma profileoption NumTemps=64
float4 _Color;
sampler2D _MainTex;
float _BlurAmount;
int _BlendQuality;
int _OffsetQuality;


struct v2f {
    float4  pos : SV_POSITION;
    float2  uv : TEXCOORD0;
};

float4 _MainTex_ST;

v2f vert (appdata_base v)
{
    v2f o;
    o.pos = UnityObjectToClipPos (v.vertex);
    o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
    return o;
}

half4 frag (v2f i) : COLOR
{
    half4 texcol = half4(0,0,0,0);
    float remaining=1.0f;
    float coef=1.0;
    float fI=0;
    float pi = 3.1415926;
    float coefDif = 1/_BlendQuality;
    float imageOffsetQty = _BlurAmount / _BlendQuality;
    for (int j = 0; j < _BlendQuality; j++) {
    	fI++;
    	coef-=coefDif;
    	float factor = coef / (_OffsetQuality*_BlendQuality);
    	float imageOffset = imageOffsetQty * fI;
    	for(float k = 0; k < _OffsetQuality; k++)
    	{
    		float angle = (k / _OffsetQuality)*pi*2;
    		float2 imgPos = float2(i.uv.x + (imageOffset * cos(angle)),i.uv.y + (imageOffset * sin(angle)));
    		texcol += tex2D(_MainTex, imgPos) * factor;
    	}
    }
    return texcol;
}
ENDCG
				
				
				
			}
		} 
	}
}