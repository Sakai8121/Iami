Shader "Custom/BurningSprite"
{
    Properties {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _DissolveTex ("Dissolve Map", 2D) = "gray" {}
        _Threshold ("Threshold", Range(0, 1.1)) = 0
    }

    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            sampler2D _DissolveTex;
            float _Threshold;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float dissolveValue = 1 - tex2D(_DissolveTex, i.uv).r;

                if (col.a < 0.01 || dissolveValue < _Threshold - 0.04) {
                    discard;
                }

                float dissolveAlpha = lerp(1, 0, 1 - saturate(abs(_Threshold - dissolveValue) / 0.04));
                col.a *= dissolveAlpha;

                return col;
            }
            ENDCG
        }
    }

    FallBack "Transparent/VertexLit"
}
