Shader "Custom/Fresnel"
{
    Properties
    { 
        _Color ("Color", Color) = (1, 1, 1, 1)    
        _Intensity ("Intensity", Range(0, 99)) = 1
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"     
            
            float4 _Color;
            float _Intensity;

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
                float3 cameraForward = -normalize(unity_MatrixV[2].xyz);
                float3 normalizedSurfNormal = normalize(IN.normalWS);
                float dotProduct = dot(cameraForward, normalizedSurfNormal);

                half alpha = 1 - abs(dotProduct);
                half4 albedo = half4(_Color * _Intensity);
                return half4(albedo.r, albedo.g, albedo.b, alpha); 
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