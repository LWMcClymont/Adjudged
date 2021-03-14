Shader "Nick/Grayscale" {
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_bwBlend ("Black and White Blend", Range (0,1)) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque" "Queue" = "Geometry+1" }
		//ColorMask 0
		ZWrite off
		
		Stencil
		{
			Ref 0
			Comp equal
		}

		Pass 
		{
			ZTest Always Cull Off ZWrite Off
				
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform fixed4 _Color;
			uniform float _bwBlend;
			half4 _MainTex_ST;

			float4 frag(v2f_img i) : COLOR
			{
				float4 c = tex2D(_MainTex, UnityStereoScreenSpaceUVAdjust(i.uv, _MainTex_ST));
				
				float lum = c.r*.3 + c.g*.59 + c.b*.11;
				float3 bw = float3(lum, lum, lum);
				
				float4 result = c;
				result.rgb = lerp(c.rgb, bw, _bwBlend);
				return result;
			}
			ENDCG
		}
	}
	Fallback off
}
