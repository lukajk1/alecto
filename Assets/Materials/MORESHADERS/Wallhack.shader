Shader "Custom/Wallhack"
{
    Properties
    { 
        _NormalColor ("Normal Color", Color) = (1, 1, 1, 1)  
        _WallhackColor ("Wallhack Color", Color) = (1, 1, 1, 1)  
    }

    SubShader
    {
        Tags { "Queue" = "Overlay" }
        
        Pass
        {
            Name "Albedo"
            ZTest Always
            ZWrite Off

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"     
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            
            float4 _NormalColor;
            float4 _WallhackColor;
            

            struct appdata
            {
                float4 positionOS   : POSITION;      
                float3 normalOS : NORMAL;
            };

            struct v2f
            {
                float4 positionHCS  : SV_POSITION;
                float3 normalWS    : TEXCOORD0;
                float4 screenPos   : TEXCOORD1;
                float3 worldPos   : TEXCOORD2;
            };            

            v2f vert(appdata IN)
            {
                v2f OUT;

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.screenPos = ComputeScreenPos(OUT.positionHCS);
                OUT.worldPos = TransformObjectToWorld(IN.positionOS.xyz);

                return OUT;
            }
            
            half4 frag(v2f IN) : SV_Target
            {
                float2 fragCoord = IN.screenPos.xy / IN.screenPos.w; // normalized screen space
                half rawDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, fragCoord);
                float linearDepth = LinearEyeDepth(rawDepth, _ZBufferParams);

                half depthSq = rawDepth * rawDepth;
                half worldPosSq = dot(IN.worldPos, IN.worldPos);

                half selector = step(worldPosSq, depthSq);
                half4 result = lerp(_NormalColor, _WallhackColor, selector);
                return result;


                //return half4(result, result, result, 1);
                //return half4(1, 1, 1, 1);
            }


            ENDHLSL
        }

        // Shadow casting pass
        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }
            
            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Back
            
            HLSLPROGRAM
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            
            // Shadow caster specific input
            float3 _LightDirection;
            
            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float2 texcoord     : TEXCOORD0;
            };
            
            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
            };
            
            Varyings ShadowPassVertex(Attributes input)
            {
                Varyings output;
                
                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);
                
                // Shadow caster specific - get position in light space and apply bias
                float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, _LightDirection));
                
                #if UNITY_REVERSED_Z
                    positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
                #else
                    positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
                #endif
                
                output.positionCS = positionCS;
                return output;
            }
            
            half4 ShadowPassFragment(Varyings input) : SV_TARGET
            {
                return 0;
            }
            ENDHLSL
        }

    }
}