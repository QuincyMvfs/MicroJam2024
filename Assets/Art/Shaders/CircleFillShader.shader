Shader "Custom/CircleFillShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "" {}            // Main texture
        _FillAmount ("Fill Amount", Range(0, 1)) = 0.5   // Fill amount
        _CircleColor ("Circle Color", Color) = (1,1,1,1) // Circle fill color
        _Opacity ("Opacity", Range(0, 1)) = 1.0          // Overall opacity
        _GlowColor ("Glow Color", Color) = (1,1,1,1)     // Glow color
        _GlowIntensity ("Glow Intensity", Range(0, 100)) = 50.0 // Glow intensity
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _FillAmount;
            float4 _CircleColor;
            float _Opacity;
            float4 _GlowColor;
            float _GlowIntensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float2 center = float2(0.5, 0.5); // Center of the UV space
                float2 uv = i.uv - center; // Offset coordinates by center
                float dist = length(uv); // Distance from center

                half4 color;
                if (dist <= _FillAmount)
                {
                    color = _CircleColor; // Inside fill area

                    // Calculate glow effect
                    float glowIntensity = smoothstep(0.0, 1.0, (_FillAmount - dist) / _FillAmount) * _GlowIntensity;
                    color.rgb += _GlowColor.rgb * glowIntensity;
                }
                else
                {
                    color = tex2D(_MainTex, i.uv); // Outside fill area
                }

                // Apply overall opacity
                color.a *= _Opacity;

                return color;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
