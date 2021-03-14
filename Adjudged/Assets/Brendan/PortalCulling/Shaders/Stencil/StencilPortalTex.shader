// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "StencilPortal/PortalTex" {
	Properties {
		_StencilTable("StencilTable", 2D) = "white" {}
		StencilRef ("Stencil Ref ID", Range(0,255)) = 1
	}
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
			
		Stencil
		{
			Ref [StencilRef]
			Comp Equal
			Pass Keep
		}

			Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			sampler2D	_StencilTable;
			float		StencilRef;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 texcoord : TEXCOORD0;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord.xy;
				return o;
			}

			half4 frag(v2f i) : COLOR
			{
				float2 coord;

				coord.x = (StencilRef % 16) / 16; // * offset;
				coord.y = 1 - (StencilRef / 16) / 16; // * offset;

				return tex2D(_StencilTable, coord + frac(i.texcoord * 32) / 16);

				return tex2D(_StencilTable, (i.texcoord * 16) + coord);
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
