Shader "Custom/GroundShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input
        {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;

        void surf(Input IN, inout SurfaceOutput o)
        {
            // Основная текстура
            fixed4 mainColor = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = mainColor.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
