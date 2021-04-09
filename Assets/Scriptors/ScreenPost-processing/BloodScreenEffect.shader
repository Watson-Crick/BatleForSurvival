Shader "Unlit/BloodScreenEffect"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BloodTex ("Blood Texture", 2D) = "white" {}
		_Transparency ("Transparency", Range(0, 1)) = 0
	}
	SubShader
	{
		Pass
		{
			ZTest Always
			Cull Off
			ZWrite Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct a2v
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
			sampler2D _BloodTex;
			float4 _BloodTex_ST;
			float _Transparency;
			
			v2f vert (a2v v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv.xy = TRANSFORM_TEX(v.uv.xy, _MainTex);
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				fixed3 col = lerp(tex2D(_MainTex, i.uv), tex2D(_BloodTex, i.uv), _Transparency);
				return fixed4(col, 1);
			}
			ENDCG
		}
	}
}
