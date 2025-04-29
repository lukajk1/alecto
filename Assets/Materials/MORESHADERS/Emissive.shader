Shader "Custom/Emissive"
{
    Properties
    { 
        _Color ("Color", Color) = (1, 1, 1, 1)  
        _Intensity ("Intensity", Range(0,99)) = 1
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
            
            float4 _Color;
            float _Intensity;

            struct Attributes
            {
                float4 positionOS   : POSITION;      
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
            };            

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }
            
            float4 frag(Varyings IN) : SV_Target
            {
                return float4(_Color * _Intensity);
            }

            ENDHLSL
        }

    }
}