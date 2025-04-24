Shader "Custom/BurningPaperWithEdgeV2"
{
    Properties {
        _MainTex ("Main texture", 2D) = "white" {}
        _DissolveTex ("Dissolution texture", 2D) = "gray" {}
        _Threshold ("Threshold", Range(0, 1.1)) = 0
        _EdgeColor ("Edge Color", Color) = (1, 0.5, 0, 1) // オレンジ
        _EdgeWidth ("Edge Width", Range(0.001, 0.1)) = 0.04
    }

    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _DissolveTex;
            float _Threshold;
            fixed4 _EdgeColor;
            float _EdgeWidth;

            v2f vert(appdata_base v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 baseColor = tex2D(_MainTex, i.uv);
                float dissolveVal = tex2D(_DissolveTex, i.uv).r; // ← 修正ポイント

                // 燃え尽きている部分 → discard
                if (dissolveVal < _Threshold - _EdgeWidth) {
                    discard;
                }

                float3 finalColor = baseColor.rgb;
                float alpha = baseColor.a;

                // 燃えている縁の範囲なら色ブレンド
                if (dissolveVal < _Threshold && dissolveVal >= _Threshold - _EdgeWidth) {
                    float t = smoothstep(_Threshold - _EdgeWidth, _Threshold, dissolveVal);
                    finalColor = lerp(_EdgeColor.rgb, baseColor.rgb, t);
                    alpha *= lerp(1, 0, 1 - t);
                }


                return fixed4(finalColor, alpha);
            }
            ENDCG
        }
    }

    FallBack "Transparent"
}
