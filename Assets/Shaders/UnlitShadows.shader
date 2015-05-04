Shader "Custom/UnlitShadow" 
{
	Properties 
	{
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
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		float _EmissiveAmount;

		struct Input 
		{
			float2 uv_MainTex;
			float2 uv;
		};

		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D( _MainTex, IN.uv_MainTex );
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
