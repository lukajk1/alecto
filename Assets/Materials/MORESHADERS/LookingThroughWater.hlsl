//#ifndef LOOKING_THROUGH_WATER_INCLUDED
//#define LOOKING_THROUGH_WATER_INCLUDED

//#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

//TEXTURE2D_X(_CameraDepthTexture);
//SAMPLER(sampler_CameraDepthTexture);

//float3 ColorBelowWater(float4 screenPos)
//{
//    float2 uv = screenPos.xy / screenPos.w;
//    float rawDepth = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, uv).r;
//    float backgroundDepth = LinearEyeDepth(rawDepth, _ZBufferParams);

//    return float3(0.0, 0.0, 0.0); // placeholder
//}

//#endif // LOOKING_THROUGH_WATER_INCLUDED