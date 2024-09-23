Shader "Custom/RedOutlineWithTransparency"
{
    Properties
    {
        _Color("Main Color", Color) = (1,1,1,1)
        _OutlineColor("Outline Color", Color) = (1,0,0,0.5) // 빨간색 + 50% 투명도
        _Outline("Outline width", Range(.002, 0.03)) = .005 // 테두리 두께
    }

        SubShader
    {
        Tags {"Queue" = "Transparent"} // 투명도 적용을 위해 Queue를 Transparent로 설정
        Pass
        {
            // 기본 오브젝트 렌더링
            Name "BASE"
            Cull Back
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha // 알파 블렌딩 활성화

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            float4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _Color;
            }
            ENDCG
        }

        Pass
        {
                // 외곽선 렌더링
                Name "OUTLINE"
                Cull Front
                ZWrite On
                Blend SrcAlpha OneMinusSrcAlpha // 알파 블렌딩 활성화

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                };

                struct v2f
                {
                    float4 pos : SV_POSITION;
                };

                float _Outline;
                float4 _OutlineColor;

                v2f vert(appdata v)
                {
                    // 법선을 사용하여 외곽선을 확대하는 방식
                    v.vertex.xyz += v.normal * _Outline;
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    return _OutlineColor; // 빨간색 + 투명도 적용
                }
                ENDCG
            }
    }

        FallBack "Diffuse"
}

