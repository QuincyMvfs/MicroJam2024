Shader "Custom/MovingNoiseGlowShader" {
    Properties {
        _MainTex ("Main Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _NoiseScale ("Noise Scale", Float) = 1
        _NoiseSpeed ("Noise Speed", Float) = 1
        _Color ("Main Color", Color) = (1, 1, 1, 1)
        _EmissionColor ("Emission Color", Color) = (1, 1, 1, 1)
        _EmissionStrength ("Emission Strength", Range(0, 10)) = 1
        _AspectRatio ("Aspect Ratio", Float) = 1
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float _NoiseScale;
            float _NoiseSpeed;
            float4 _Color;
            float4 _EmissionColor;
            float _EmissionStrength;
            float _AspectRatio;

            v2f vert (appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // Calculate aspect ratio scaling
                float2 scaledUV = i.uv;
                scaledUV.x *= _AspectRatio;

                // Calculate moving noise UVs
                float2 noiseUV = scaledUV * _NoiseScale;
                noiseUV += _Time.y * _NoiseSpeed;

                // Sample the main texture and noise texture
                fixed4 mainTexColor = tex2D(_MainTex, i.uv);
                fixed4 noiseTexColor = tex2D(_NoiseTex, noiseUV);

                // Combine textures (for this example, we multiply them)
                fixed4 combinedColor = mainTexColor * noiseTexColor * _Color;

                // Add emission to the combined color
                fixed4 emission = _EmissionColor * _EmissionStrength;

                // Final color with emission
                fixed4 finalColor = combinedColor + emission;

                return finalColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
