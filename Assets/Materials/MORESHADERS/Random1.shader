Shader "Custom/Random1"
{
    Properties
    { 
        _Alpha ("Alpha", Range(0, 1)) = 0.5   
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
            
            half _Alpha;

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
                // Normalize surface normal
                float3 N = normalize(IN.normalWS);

                // Compute view direction (from pixel to camera)
                float3 viewDirWS = normalize(GetWorldSpaceViewDir(IN.normalWS.xyz));

                // Dot product: cosine of the angle
                float NdotV = dot(N, viewDirWS);

                // Optionally get actual angle
                float angleRadians = acos(clamp(NdotV, -1.0, 1.0));
                float angleDegrees = degrees(angleRadians);

                // For example: use NdotV to control brightness
                float brightness = saturate(NdotV); // 0 if perpendicular, 1 if directly facing camera

                // Output color: brighter when facing camera
                return half4(brightness.xxx, 1.0); // Grayscale brightness
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