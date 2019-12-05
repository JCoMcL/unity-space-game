Shader "Custom/Test/Shimmer" {
Properties {
	_MainTex ("Texture", 2D) = "white" {}
}

SubShader {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }

	Blend SrcAlpha OneMinusSrcAlpha
	GrabPass{"_GrabTex"}

	Pass {
	
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"

		sampler2D _MainTex;

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

		fixed4 frag (v2f i) : SV_Target
		{
			i.uvgrab.xy += (tex2D(_MainTex, i.texcoord).r/100*sin(_Time.y));
			fixed4 col = tex2D(_GrabTex, i.uvgrab);
			return col;
		}
		ENDCG 
	}

}
}
