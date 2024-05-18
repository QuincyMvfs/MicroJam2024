Shader "Custom/TeleportShader" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _DistortionAmount ("Distortion Amount", Range(0, 1)) = 0.1
        _DistortionFrequency ("Distortion Frequency", Range(0, 10)) = 1
        _DissolveAmount ("Dissolve Amount", Range(0, 1)) = 0
        _DissolveColor ("Dissolve Color", Color) = (0,0,0,1)
        _DissolveTexture ("Dissolve Texture", 2D) = "white" {}
        _GlowColor ("Glow Color", Color) = (1,1,1,1)
        _GlowIntensity ("Glow Intensity", Range(0, 10)) = 1
    }
    SubShader {
        Tags { "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // Include UnityCG for built-in shader functions
            #include "UnityCG.cginc"

            // Define shader properties
            sampler2D _MainTex;
            sampler2D _DissolveTexture;
            float _DistortionAmount;
            float _DistortionFrequency;
            float _DissolveAmount;
            fixed4 _DissolveColor;
            float4 _GlowColor;
            float _GlowIntensity;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                // Sample the dissolve texture
                float dissolve = tex2D(_DissolveTexture, i.uv).r;
                // Apply dissolve effect
                if (dissolve < _DissolveAmount) {
                    // Calculate glow effect
                    float glowFactor = pow(1 - dissolve, _GlowIntensity);
                    fixed4 glow = _GlowColor * glowFactor;
                    // Apply glow to the dissolve color
                    return _DissolveColor + glow;
                }
                
                // Sample the main texture with distortion
                float2 distortedUV = i.uv + _DistortionAmount * sin(i.uv * _DistortionFrequency + _Time.y);
                fixed4 col = tex2D(_MainTex, distortedUV);
                // Modify the color or add effects here
                return col;
            }
            ENDCG
        }
    }
}