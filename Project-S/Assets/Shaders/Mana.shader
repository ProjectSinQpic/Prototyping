Shader "Custom/Mana" {
    Properties{
        _BaseColor("Base Color", Color) = (1,1,1,0.5)
        _RimColor("Rim Color", Color) = (1,1,1,0.5)
    }
	SubShader {
		Tags { 
            "Queue" = "Transparent"
        }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard alpha:fade
		#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
			float3 worldNormal;
      		float3 viewDir;
		};

        float4 _BaseColor;
        float4 _RimColor;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			o.Albedo = _BaseColor;
			float rim = 1 - saturate(dot(IN.viewDir, o.Normal));
     		o.Emission = _RimColor * pow(rim, 2.5);
            float alpha = 1 - (abs(dot(IN.viewDir, IN.worldNormal))) * 0.75f;
     		o.Alpha =  alpha*2.0f;
		}
		ENDCG
	}
	FallBack "Diffuse"
}