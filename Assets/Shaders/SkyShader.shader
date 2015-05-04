Shader "Custom/SkyShader" 
{
	Properties 
	{
		_TopColor ("TopColor", Color) = (1,1,1,1)
		_BottomColor ("BottomColor", Color) = (1,1,1,1)
		_MainTex ("MainTex", 2D ) = "white" {}
		
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Lighting Off
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf NoLighting

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input 
		{
			float2 uv_MainTex;
			float2 uv;
		};

		//half _Glossiness;
		//half _Metallic;
		fixed4 _Color;
		fixed4 _TopColor;
		fixed4 _BottomColor;

		fixed4 LightingNoLighting (SurfaceOutput s, UnityGI gi)
		{
			fixed4 c = (s.Albedo, 1.0);
			return c;
		}
		
		void LightingNoLighting_GI (
			SurfaceOutput s,
			UnityGIInput data,
			inout UnityGI gi)
		{
			gi = UnityGlobalIllumination (data, 1.0, 0.0, s.Normal, false);
		}

		void surf (Input IN, inout SurfaceOutput o) 
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = lerp( _BottomColor, _TopColor, IN.uv_MainTex.y );
			o.Albedo = c.rgb;
			o.Emission = c.rgb;
			
			// Metallic and smoothness come from slider variables
			//o.Metallic = 0;
			//o.Smoothness = 0;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
