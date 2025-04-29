Shader "Custom/TimeScrollingColor"
{
    Properties
    { 
        _ScrollSpeed ("ScrollSpeed", Range(0, 10)) = 1  
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"     
            
            half _ScrollSpeed;

            struct Attributes
            {
                float4 positionOS   : POSITION;      
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float3 normalWS    : TEXCOORD0;
            };            

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                return OUT;
            }
            
            half4 frag(Varyings IN) : SV_Target
            {
                float time = _Time.y * _ScrollSpeed;
                float rVal = 0.5 * sin(time) + 0.5;
                float gVal = 0.5 * cos(time) + 0.5;
                float bVal = 0.5 * cos(time + 3.14) + 0.5;

                half3 scroll = half3(rVal, gVal, bVal);
                return half4(scroll, 1.0);
            }


            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment fragShadowCaster

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            half4 fragShadowCaster(Varyings IN) : SV_Target
            {
                return 0;
            }
            ENDHLSL
        }

    }
}