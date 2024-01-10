Shader "Custom/TextureBlendingShader" {
    Properties {
        _MainTex1 ("Texture 1", 2D) = "white" {}
        _MainTex2 ("Texture 2", 2D) = "white" {}
        _MainTex3 ("Texture 3", 2D) = "white" {}
        _MainTex4 ("Texture 4", 2D) = "white" {}
        _BlendWeight1 ("Blend Weight 1", Range(0, 1)) = 1
        _BlendWeight2 ("Blend Weight 2", Range(0, 1)) = 1
        _BlendWeight3 ("Blend Weight 3", Range(0, 1)) = 1
        _BlendWeight4 ("Blend Weight 4", Range(0, 1)) = 1
        _NoiseScale ("Noise Scale", Range(0, 0.1)) = 0.05
        _IslandsDensity ("Islands Density", Range(0, 1)) = 0.1
        [MaterialToggle] _AutoDensity ("Auto Density", Float) = 0
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

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
        float _BlendWeight1;
        float _BlendWeight2;
        float _BlendWeight3;
        float _BlendWeight4;
        float _NoiseScale;
        float _IslandsDensity;
        float _AutoDensity;

        float PerlinNoise(float2 uv) {
            float2 p = uv * _NoiseScale;
            return tex2D(_MainTex1, p).r;
        }

        void surf(Input IN, inout SurfaceOutput o) {
            fixed4 col1 = tex2D(_MainTex1, IN.uv_MainTex1);
            fixed4 col2 = tex2D(_MainTex2, IN.uv_MainTex2);
            fixed4 col3 = tex2D(_MainTex3, IN.uv_MainTex3);
            fixed4 col4 = tex2D(_MainTex4, IN.uv_MainTex4);

            float totalWeight = _BlendWeight1 + _BlendWeight2 + _BlendWeight3 + _BlendWeight4;

            fixed4 finalColor = col1 * (_BlendWeight1 / totalWeight);
            finalColor += col2 * (_BlendWeight2 / totalWeight);
            finalColor += col3 * (_BlendWeight3 / totalWeight);
            finalColor += col4 * (_BlendWeight4 / totalWeight);

            if (_AutoDensity > 0) {
                float maxWeight = max(max(max(_BlendWeight1, _BlendWeight2), _BlendWeight3), _BlendWeight4);
                float sumWeights = totalWeight - maxWeight;
                _IslandsDensity = maxWeight - (sumWeights / 3.0);
            }

            // Add random islands using Perlin noise
            float noise = PerlinNoise(IN.uv_MainTex1);
            float islands = step(noise, _IslandsDensity);

            // Select the texture with the highest blend weight for empty areas
            fixed4 highestWeightTexture = col1;
            float highestWeight = _BlendWeight1;

            if (_BlendWeight2 > highestWeight) {
                highestWeightTexture = col2;
                highestWeight = _BlendWeight2;
            }

            if (_BlendWeight3 > highestWeight) {
                highestWeightTexture = col3;
                highestWeight = _BlendWeight3;
            }

            if (_BlendWeight4 > highestWeight) {
                highestWeightTexture = col4;
                highestWeight = _BlendWeight4;
            }

            finalColor = lerp(finalColor, highestWeightTexture, islands); // Use the texture with the highest blend weight for islands

            o.Albedo = finalColor.rgb;
            o.Alpha = finalColor.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
