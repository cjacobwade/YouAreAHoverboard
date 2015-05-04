Shader "Custom/SmokeShader" 
{
	Properties 
	{
		_TopColor ("TopColor", Color) = (1,1,1,1)
		_BottomColor ("BottomColor", Color) = (1,1,1,1)
		_MainTex ("MainTex", 2D ) = "white" {}
		
		_EmissiveAmount ( "Emission Divisor", float ) = 3
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		Lighting Off
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		float _EmissiveAmount;

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

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = lerp( _BottomColor, _TopColor, IN.uv_MainTex.y );
			o.Albedo = c.rgb;
			o.Emission = c.rgb/_EmissiveAmount;
			
			// Metallic and smoothness come from slider variables
			o.Metallic = 0;
			o.Smoothness = 0;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
