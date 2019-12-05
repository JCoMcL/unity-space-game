Shader "Custom/MappedBlur" {
Properties {
	_BlurTex ("Blur Map", 2D) = "white" {}
	_MainTex ("Shade Texture", 2D) = "alpha" {}
	_Color ("Tint", Color) = (1,0,0,1.5)
	_BlurAmount ("Blur Amount", Range(0,1)) = 0.0005
	_BlendQuality("Blend Quality", int) = 3
	_OffsetQuality("Offset Quality", int) = 4
}

SubShader {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }


	GrabPass{"_GrabTex"}

	Pass {
		Blend One Zero
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		#pragma profileoption NumTemps=64
		sampler2D _BlurTex;
		float _BlurAmount;
		int _BlendQuality;
		int _OffsetQuality;

		struct appdata_t {
			float4 vertex : POSITION;
			fixed4 color : COLOR;
			float2 texcoord : TEXCOORD0;
		};

		struct v2f {
			float4 vertex : SV_POSITION;
			fixed4 uvgrab : TEXCOORD1;
			float2 texcoord : TEXCOORD0;
		};
		
		float4 _MainTex_ST;
		sampler2D _GrabTex;

		v2f vert (appdata_t v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.uvgrab = ComputeGrabScreenPos(o.vertex);
			o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
			return o;
		}

		half4 frag (v2f i) : SV_Target
		{
			half4 texcol = half4(0,0,0,0);
		    float remaining=1.0f;
		    float coef=1.0;
		    float fI=0;
		    float pi = 3.1415926;
		    float coefDif = 1/_BlendQuality;
		    float imageOffsetQty = _BlurAmount*tex2D(_BlurTex, i.texcoord).a / _BlendQuality;
		    for (int j = 0; j < _BlendQuality; j++) {
		    	fI++;
		    	coef-=coefDif;
		    	float factor = coef / (_OffsetQuality*_BlendQuality);
		    	float imageOffset = imageOffsetQty * fI;
		    	for(float k = 0; k < _OffsetQuality; k++)
		    	{
		    		float angle = (k / _OffsetQuality)*pi*2;
		    		float2 imgPos = float2(i.uvgrab.x + (imageOffset * cos(angle)),i.uvgrab.y + (imageOffset * sin(angle)));
		    		texcol += tex2D(_GrabTex, imgPos) * factor;
		    	}
		    }
		    return texcol;
		}
		ENDCG 
	}
	Pass {
		Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		#pragma profileoption NumTemps=64
		float4 _Color;
		sampler2D _MainTex;

		struct appdata_t {
			float4 vertex : POSITION;
			fixed4 color : COLOR;
			float2 texcoord : TEXCOORD0;
		};

		struct v2f {
			float4 vertex : SV_POSITION;
			float2 texcoord : TEXCOORD0;
		};
		
		float4 _MainTex_ST;

		v2f vert (appdata_t v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
			return o;
		}

		fixed4 frag (v2f i) : SV_Target
		{
		    return tex2D(_MainTex, i.texcoord) * _Color;
		}
		ENDCG 
	}

}
}
