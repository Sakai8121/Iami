Shader "Custom/BurningPaper"
{
    Properties {
        _MainTex ("Main texture", 2D) = "white" {}
        _DissolveTex ("Dissolution texture", 2D) = "gray" {}
        _Threshold ("Threshold", Range(0, 1.1)) = 0
    }

    SubShader {
        // **透過用の設定**
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

            v2f vert(appdata_base v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 c = tex2D(_MainTex, i.uv);
                fixed val = 1 - tex2D(_DissolveTex, i.uv).r;

                // **完全に透明な部分は削除**
                if (c.a < 0.01 || val < _Threshold - 0.04) {
                    discard;
                }

                bool b = val < _Threshold;
                float dissolveAlpha = lerp(1, 0, 1 - saturate(abs(_Threshold - val) / 0.04));

                // **元のアルファ値を考慮して透明度を適用**
                c.a *= dissolveAlpha;

                return c;
            }
            ENDCG
        }
    }
    FallBack "Transparent"
}
