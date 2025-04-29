Shader "Custom/GithubOutline" 
{
    Properties 
    {
        _Thickness ("Thickness", Range(-0.2,0.2)) = 0.07
        _OutlineColor ("Outline Color", Color) = (1, 1, 1, 1)
        _DepthOffset("Depth offset", Range(0,1)) = 0 // An offset to the clip space Z, pushing the outline back
        [Toggle(RAINBOWOUTLINE)]_RainbowOutline ("UseRainbowOutline", Float) = 0
        _ScrollSpeed("Rainbow Scroll Speed", Range(0, 10)) = 1
        // If enabled, this shader will use "smoothed" normals stored in TEXCOORD1 to extrude along
        [Toggle(USE_PRECALCULATED_OUTLINE_NORMALS)]_PrecalculateNormals("Use UV1 normals", Float) = 0
    }
    SubShader 
    {
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" }

        // Outline pass only
        Pass {
            Name "Outlines"
            // Cull front faces
            Cull Front
            
            HLSLPROGRAM
            // Standard URP requirements
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9xa
            #pragma shader_feature_local RAINBOWOUTLINE
            // Register our material keywords
            #pragma shader_feature_local USE_PRECALCULATED_OUTLINE_NORMALS
            // Register our functions
            #pragma vertex Vertex
            #pragma fragment Fragment
            
            // Include required libraries
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
            
            CBUFFER_START(UnityPerMaterial)
                float _Thickness;
                float4 _OutlineColor;
                float _DepthOffset;
                float _ScrollSpeed;
            CBUFFER_END
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL; 
                #ifdef USE_PRECALCULATED_OUTLINE_NORMALS
                    float3 smoothNormalOS   : TEXCOORD1; // Calculated "smooth" normals to extrude along in object space
                #endif
            };
            
            struct VertexOutput
            {
                float4 positionCS : SV_POSITION;
            };
            
            VertexOutput Vertex(Attributes input)
            {
                VertexOutput output = (VertexOutput) 0;
                float3 normalOS = input.normalOS;
                
                #ifdef USE_PRECALCULATED_OUTLINE_NORMALS
                    normalOS = input.smoothNormalOS;
                #else
                    normalOS = input.normalOS;
                #endif
                
                // Extrude the object space position along a normal vector
                float3 posOS = input.positionOS.xyz + normalOS * _Thickness;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(posOS);
                output.positionCS = vertexInput.positionCS;
                
                float depthOffset = _DepthOffset;
                // If depth is reversed on this platform, reverse the offset
                #ifdef UNITY_REVERSED_Z
                    depthOffset = -depthOffset;
                #endif
                output.positionCS.z += depthOffset;
                return output;
            }
            
            float4 Fragment(VertexOutput input) : SV_Target
            {
                #ifdef RAINBOWOUTLINE
                    float time = _Time.y * _ScrollSpeed;
                    float rVal = 0.5 * sin(time) + 0.5;
                    float gVal = 0.5 * cos(time) + 0.5;
                    float bVal = 0.5 * cos(time + 3.14) + 0.5;
                    return float4(rVal, gVal, bVal, 1.0);
                #else
                    return _OutlineColor;
                #endif
            }
            ENDHLSL
        }
    }
}