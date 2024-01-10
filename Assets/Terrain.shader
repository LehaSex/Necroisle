Shader "Custom/TextureBlendingShader" {
    Properties {
        _MainTex1 ("Texture 1", 2D) = "white" {}
        _MainTex2 ("Texture 2", 2D) = "white" {}
        _MainTex3 ("Texture 3", 2D) = "white" {}
        _MainTex4 ("Texture 4", 2D) = "white" {}
        _BlendRatio1 ("Blend Ratio 1", Range(0, 1)) = 0.25
        _BlendRatio2 ("Blend Ratio 2", Range(0, 1)) = 0.25
        _BlendRatio3 ("Blend Ratio 3", Range(0, 1)) = 0.25
        _BlendRatio4 ("Blend Ratio 4", Range(0, 1)) = 0.25
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        struct Input {
            float2 uv_MainTex1;
            float2 uv_MainTex2;
            float2 uv_MainTex3;
            float2 uv_MainTex4;
        };

        sampler2D _MainTex1;
        sampler2D _MainTex2;
        sampler2D _MainTex3;
        sampler2D _MainTex4;
        float _BlendRatio1;
        float _BlendRatio2;
        float _BlendRatio3;
        float _BlendRatio4;

        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf(Input IN, inout SurfaceOutputStandard o) {
            fixed4 col1 = tex2D(_MainTex1, IN.uv_MainTex1);
            fixed4 col2 = tex2D(_MainTex2, IN.uv_MainTex2);
            fixed4 col3 = tex2D(_MainTex3, IN.uv_MainTex3);
            fixed4 col4 = tex2D(_MainTex4, IN.uv_MainTex4);

            fixed4 finalColor = col1 * _BlendRatio1 + col2 * _BlendRatio2 + col3 * _BlendRatio3 + col4 * _BlendRatio4;

            o.Albedo = finalColor.rgb;
            o.Alpha = finalColor.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}