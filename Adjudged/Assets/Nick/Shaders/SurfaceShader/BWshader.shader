Shader "Nick/BWshader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Metallic ("Metallic (BW)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		[Toggle] _EnableBW("Black and White", Float) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows finalcolor:BlackWhite

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float2 uv_Metallic;
			
		};

		sampler2D _Metallic;
		half _Glossiness;
		float _EnableBW;
		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			fixed4 metal = tex2D(_Metallic, IN.uv_Metallic);
			o.Metallic = metal.r;
			o.Smoothness = metal * _Glossiness;
			o.Alpha = c.a;
		}

		void BlackWhite(Input IN, SurfaceOutputStandard o, inout fixed4 color)
		{
			if (_EnableBW)
			{
				float lum = color.r*.3 + color.g*.59 + color.b*.11;
				float3 bw = float3(lum, lum, lum);
				float4 result = float4(bw, color.a);
				color = result;
			}
			else
				return;
			
		}

		ENDCG
	}
	FallBack "Diffuse"
}
