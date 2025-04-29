Shader "Custom/Transparent1"
{
    Properties
    { 
        _Alpha ("Alpha", Range(0, 1)) = 0.5   
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
ZWrite Off

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
                half3 normalColor = IN.normalWS * 0.5 + 0.5;
                return half4(normalColor, _Alpha);
            }
            ENDHLSL
        }
    }
}