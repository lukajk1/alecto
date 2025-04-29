Shader "Custom/InverseDepthTexture"
{
    Properties
    { 
        _Color ("Color", Color) = (1, 1, 1, 1)  
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        ZWrite On

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"     
            
            float4 _Color;
            
            TEXTURE2D(_CameraDepthTexture);
            SAMPLER(sampler_CameraDepthTexture);

            struct Attributes
            {
                float4 positionOS   : POSITION;      
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float3 normalWS    : TEXCOORD0;
                float4 screenPos   : TEXCOORD1;
            };            

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.screenPos = ComputeScreenPos(OUT.positionHCS);
                return OUT;
            }
            
            half4 frag(Varyings IN) : SV_Target
            {    
                float2 fragCoord = IN.screenPos.xy / IN.screenPos.w; // normalized screen space
                half depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, fragCoord) * 4;
                half depth2 = 1 -  (4.6 * depth);
                return half4(depth2, depth2, depth2, 1.0);  
            }


            ENDHLSL
        }


    }
}