Shader "Custom/Rimlight"
{
    Properties
    { 
        _Albedo ("local color", Color) = (1, 1, 1, 1) 

        [Header(rimlight)]
        _RimlightColor ("RimlightColor", Color) = (1, 1, 1, 1)    
        _Intensity ("rimlight intensity", Range(0, 99)) = 1
        _DotProductThreshold ("DotProductThreshold", Range(0.001, 1.3)) = 0.9
        
        [Header(ambient light)]
        _AmbientLightColor ("ambient light color", Color) = (0, 0, 1, 1)    
        _AmbientStrength ("ambient light strength", Range(0,1)) = 0.5
        _AmbientThreshold ("ambient light threshold", Range(-5,10)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        Pass
        {
            Name "Rimlight"

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"   
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"   
            
            float4 _RimlightColor;
            float4 _AmbientLightColor;
            float _Intensity;
            float _DotProductThreshold;

            float4 _Albedo;

            float _AmbientStrength;
            float _AmbientThreshold;

            struct appdata
            {
                float4 positionOS   : POSITION;      
                float3 normalOS : NORMAL;
            };

            struct v2f
            {
                float4 positionHCS  : SV_POSITION;
                float3 normalWS    : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };            

            v2f vert(appdata IN)
            {
                v2f OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                return OUT;
            }
            
            half4 frag(v2f IN) : SV_Target
            {
                Light mainLight = GetMainLight();

                float3 overheadPoint = 1- _WorldSpaceCameraPos + float3(-1,1,1);

                // normalize normal at fragment
                float3 normalizedSurfaceNormal = normalize(IN.normalWS);

                // rimlight calculation
                float3 cameraForwardToPoint = normalize(_WorldSpaceCameraPos - IN.worldPos);
                float3 cameraForward = -normalize(unity_MatrixV[2].xyz);


                float dotProduct = dot(overheadPoint, normalizedSurfaceNormal);

                half clamped = clamp(dotProduct, 0, 1);
                half result = step(_DotProductThreshold, clamped);

                half4 rim = half4(_RimlightColor * _Intensity);

                // albedo + ambient calculation
                half4 diffuse = half4(_Albedo);
                float3 down = float3(0,-1,0);
                float ambientDot = dot(down, normalizedSurfaceNormal);
                float ambientFactor = saturate((ambientDot - _AmbientThreshold) / (1.0 - _AmbientThreshold));
                half4 lerpDiffuse = lerp(_AmbientLightColor, _Albedo, ambientFactor);

                // put together
                half4 finalColor = lerp(lerpDiffuse, rim, result);
                return half4(finalColor);
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

            struct appdata
            {
                float4 positionOS : POSITION;
            };

            struct v2f
            {
                float4 positionCS : SV_POSITION;
            };

            v2f vert(appdata IN)
            {
                v2f OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            half4 fragShadowCaster(v2f IN) : SV_Target
            {
                return 0;
            }
            ENDHLSL
        }

    }
}