Shader "Custom/AtkTransparentShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {} // 텍스처 설정
        _Color("Color", Color) = (1, 1, 1, 0.5) // 초기 투명도 설정
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 200

            Pass
            {
                // 투명도 설정 및 깊이 쓰기 활성화
                Blend SrcAlpha OneMinusSrcAlpha // 알파 블렌딩
                ZWrite On // 깊이 쓰기 활성화
                Cull Back // 뒷면 컬링

                // 셰이더 코드 시작
                HLSLPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                    float3 worldNormal : TEXCOORD1;
                    float3 worldPos : TEXCOORD2;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _Color;

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    o.worldNormal = mul((float3x3)unity_ObjectToWorld, v.normal);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // Lambert 조명 계산
                    fixed3 lightDir = normalize(_WorldSpaceLightPos0.xyz); // 광원의 방향
                    float NdotL = max(0, dot(normalize(i.worldNormal), lightDir)); // 법선과 광원 방향의 내적

                    // 조명에 따른 명암과 텍스처 색상 적용
                    fixed4 texColor = tex2D(_MainTex, i.uv); // 텍스처 색상
                    texColor.rgb *= _Color.rgb; // 텍스처 색상에 머티리얼 색상 곱하기
                    texColor.rgb = texColor.rgb * NdotL + 0.1; // 조명에 따른 밝기 조정, 최소값 추가하여 완전한 검은색 방지

                    // 투명도 적용
                    texColor.a *= _Color.a; // 알파 값 적용 (투명도 반영)

                    return texColor;
                }
                ENDHLSL
            }
        }
            FallBack "Transparent/Diffuse"
}


