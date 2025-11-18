#extension GL_OES_shader_image_atomic : require
layout(local_size_x = 8, local_size_y = 8, local_size_z = 1) in;

layout(binding = 0, std140) uniform type_cbFSR3Upscaler
{
    ivec2 iRenderSize;
    ivec2 iPreviousFrameRenderSize;
    ivec2 iUpscaleSize;
    ivec2 iPreviousFrameUpscaleSize;
    ivec2 iMaxRenderSize;
    ivec2 iMaxUpscaleSize;
    vec4 fDeviceToViewDepth;
    vec2 fJitter;
    vec2 fPreviousFrameJitter;
    vec2 fMotionVectorScale;
    vec2 fDownscaleFactor;
    vec2 fMotionVectorJitterCancellation;
    float fTanHalfFOV;
    float fJitterSequenceLength;
    float fDeltaTime;
    float fDeltaPreExposure;
    float fViewSpaceToMetersFactor;
    float fFrameIndex;
    float fVelocityFactor;
    float onlycopy;
} cbFSR3Upscaler;

layout(binding = 1, std140) uniform type_cbSceneInfo
{
    mat4 transform;
} cbSceneInfo;

layout(binding = 0, rgba8) uniform writeonly highp image2D rw_internal_upscaled_color;
layout(binding = 4, r32ui) uniform highp uimage2D rw_game_motion_vector_field_x;
uniform highp sampler2D SPIRV_Cross_Combinedr_input_depthSPIRV_Cross_DummySampler;
uniform highp sampler2D SPIRV_Cross_Combinedr_dilated_motion_vectorsSPIRV_Cross_DummySampler;
uniform highp sampler2D SPIRV_Cross_Combinedr_internal_upscaled_colorSPIRV_Cross_DummySampler;
uniform highp sampler2D SPIRV_Cross_Combinedr_input_color_jitteredSPIRV_Cross_DummySampler;

ivec2 _211;

void main()
{
    if (cbFSR3Upscaler.onlycopy > 0.100000001490116119384765625)
    {
        ivec2 _916 = ivec2(floor((vec2(ivec3(gl_GlobalInvocationID).xy) + vec2(0.5)) * cbFSR3Upscaler.fDownscaleFactor));
        ivec2 _924 = _916;
        _924.x = max(1, min(_916.x, (cbFSR3Upscaler.iRenderSize.x - 2)));
        ivec2 _930 = _924;
        _930.y = max(1, min(_916.y, (cbFSR3Upscaler.iRenderSize.y - 2)));
        imageStore(rw_internal_upscaled_color, ivec2(uvec2(ivec3(gl_GlobalInvocationID).xy)), vec4(texelFetch(SPIRV_Cross_Combinedr_input_color_jitteredSPIRV_Cross_DummySampler, ivec2(uvec2(_930)), int(0u)).xyz, 1.0));
    }
    else
    {
        uvec2 _237;
        vec2 _222 = vec2(ivec3(gl_GlobalInvocationID).xy) + vec2(0.5);
        vec2 _225 = vec2(cbFSR3Upscaler.iUpscaleSize);
        vec2 _226 = _222 / _225;
        vec2 _229 = vec2(cbFSR3Upscaler.iRenderSize);
        uvec2 _231 = uvec2(_226 * _229);
        vec4 _233 = texelFetch(SPIRV_Cross_Combinedr_input_depthSPIRV_Cross_DummySampler, ivec2(_231), int(0u));
        float _234 = _233.x;
        vec2 _278;
        switch (0u)
        {
            default:
            {
                _237 = uvec2(cbFSR3Upscaler.iRenderSize);
                if (all(lessThan(_231, _237)))
                {
                    vec4 _243 = texelFetch(SPIRV_Cross_Combinedr_dilated_motion_vectorsSPIRV_Cross_DummySampler, ivec2(_231), int(0u));
                    vec3 _276;
                    if (_243.z < 0.001000000047497451305389404296875)
                    {
                        vec2 _257 = (vec2(_231) + vec2(0.5)) / _229;
                        vec4 _267 = cbSceneInfo.transform * vec4((_257 * vec2(2.0)) - vec2(1.0), (2.0 * _234) - 1.0, 1.0);
                        vec2 _274 = (((_267.xy / vec2(_267.w)) + vec2(1.0)) * vec2(0.5)) - _257;
                        _276 = vec3(_274.x, _274.y, _243.z);
                    }
                    else
                    {
                        vec2 _253 = ((_243.xy - vec2(0.4999924004077911376953125)) * vec2(4.008016109466552734375)).xy * vec2(-0.5);
                        _276 = vec3(_253.x, _253.y, _243.z);
                    }
                    _278 = _276.xy;
                    break;
                }
                _278 = vec2(0.0);
                break;
            }
        }
        float _280 = _234 * (-999.0);
        vec2 _294 = _278 * 0.5;
        ivec2 _298 = ivec2(floor(((vec2(ivec2(_231)) * (vec2(1.0) / _229)) + _294) * _229));
        if (all(lessThan(_298, cbFSR3Upscaler.iRenderSize)) && all(greaterThan(_298, ivec2(0))))
        {
            vec2 _307 = (_294 + vec2(1.0)) * vec2(0.5);
            uint _324 = imageAtomicMax(rw_game_motion_vector_field_x, ivec2(uvec2(uint(_298.x), uint(_298.y))), ((((2147483648u | ((max(1u, uint(((_280 + 1000.0) / (_280 + 2000.0)) * 2046.0)) & 1023u) << 21u)) | 0u) & 4292870144u) | (uint(_307.x * 2047.0) << 10u)) | uint(_307.y * 1023.0));
        }
        vec2 _326 = _226 + (_278 * vec2(0.5));
        float _327 = _326.x;
        float _331 = _326.y;
        bool _335 = ((_327 >= 0.0) && (_327 <= 1.0)) && ((_331 >= 0.0) && (_331 <= 1.0));
        vec3 _421;
        if (_335 && (!((_335 == false) || (0.0 == cbFSR3Upscaler.fFrameIndex))))
        {
            vec2 _346 = (_326 * _225) - vec2(0.5);
            vec2 _348 = _346 - floor(_346);
            vec2 _354 = _346;
            _354.x = max(0.0, min(float(cbFSR3Upscaler.iUpscaleSize.x), _346.x));
            vec2 _360 = _354;
            _360.y = max(0.0, min(float(cbFSR3Upscaler.iUpscaleSize.y), _346.y));
            ivec2 _362 = ivec2(floor(_360));
            ivec2 _367 = _362;
            _367.x = max(1, min(_362.x, (cbFSR3Upscaler.iUpscaleSize.x - 2)));
            ivec2 _372 = _367;
            _372.y = max(1, min(_362.y, (cbFSR3Upscaler.iUpscaleSize.y - 2)));
            vec4 _387 = texelFetch(SPIRV_Cross_Combinedr_internal_upscaled_colorSPIRV_Cross_DummySampler, ivec2(uvec2(_372)), int(0u));
            vec4 _390 = texelFetch(SPIRV_Cross_Combinedr_internal_upscaled_colorSPIRV_Cross_DummySampler, ivec2(uvec2(_372 + ivec2(1, 0))), int(0u));
            vec4 _396 = texelFetch(SPIRV_Cross_Combinedr_internal_upscaled_colorSPIRV_Cross_DummySampler, ivec2(uvec2(_372 + ivec2(0, 1))), int(0u));
            vec4 _399 = texelFetch(SPIRV_Cross_Combinedr_internal_upscaled_colorSPIRV_Cross_DummySampler, ivec2(uvec2(_372 + ivec2(1))), int(0u));
            mediump float _18 = _348.x;
            mediump float _21 = (-1.0) - _18;
            mediump float _33 = _21 * _21;
            mediump float _35 = (0.39990234375 * _33) - 1.0;
            mediump float _37 = (0.25 * _33) - 1.0;
            mediump float _42 = (((1.5625 * _35) * _35) - 0.5625) * (_37 * _37);
            mediump float _22 = -_18;
            mediump float _43 = _22 * _22;
            mediump float _45 = (0.39990234375 * _43) - 1.0;
            mediump float _47 = (0.25 * _43) - 1.0;
            mediump float _52 = (((1.5625 * _45) * _45) - 0.5625) * (_47 * _47);
            mediump float _23 = 1.0 - _18;
            mediump float _53 = _23 * _23;
            mediump float _55 = (0.39990234375 * _53) - 1.0;
            mediump float _57 = (0.25 * _53) - 1.0;
            mediump float _62 = (((1.5625 * _55) * _55) - 0.5625) * (_57 * _57);
            mediump vec4 _31 = vec4((_42 + _52) + _62);
            mediump float _19 = _348.y;
            mediump float _75 = (-1.0) - _19;
            mediump float _87 = _75 * _75;
            mediump float _89 = (0.39990234375 * _87) - 1.0;
            mediump float _91 = (0.25 * _87) - 1.0;
            mediump float _96 = (((1.5625 * _89) * _89) - 0.5625) * (_91 * _91);
            mediump float _76 = -_19;
            mediump float _97 = _76 * _76;
            mediump float _99 = (0.39990234375 * _97) - 1.0;
            mediump float _101 = (0.25 * _97) - 1.0;
            mediump float _106 = (((1.5625 * _99) * _99) - 0.5625) * (_101 * _101);
            mediump float _77 = 1.0 - _19;
            mediump float _107 = _77 * _77;
            mediump float _109 = (0.39990234375 * _107) - 1.0;
            mediump float _111 = (0.25 * _107) - 1.0;
            mediump float _116 = (((1.5625 * _109) * _109) - 0.5625) * (_111 * _111);
            vec3 _405 = (clamp((((((((texelFetch(SPIRV_Cross_Combinedr_internal_upscaled_colorSPIRV_Cross_DummySampler, ivec2(uvec2(_372 + ivec2(-1))), int(0u)) * _42) + (texelFetch(SPIRV_Cross_Combinedr_internal_upscaled_colorSPIRV_Cross_DummySampler, ivec2(uvec2(_372 + ivec2(0, -1))), int(0u)) * _52)) + (texelFetch(SPIRV_Cross_Combinedr_internal_upscaled_colorSPIRV_Cross_DummySampler, ivec2(uvec2(_372 + ivec2(1, -1))), int(0u)) * _62)) / _31) * _96) + (((((texelFetch(SPIRV_Cross_Combinedr_internal_upscaled_colorSPIRV_Cross_DummySampler, ivec2(uvec2(_372 + ivec2(-1, 0))), int(0u)) * _42) + (_387 * _52)) + (_390 * _62)) / _31) * _106)) + (((((texelFetch(SPIRV_Cross_Combinedr_internal_upscaled_colorSPIRV_Cross_DummySampler, ivec2(uvec2(_372 + ivec2(-1, 1))), int(0u)) * _42) + (_396 * _52)) + (_399 * _62)) / _31) * _116)) / vec4((_96 + _106) + _116), min(min(min(_387, _390), _396), _399), max(max(max(_387, _390), _396), _399)).xyz * cbFSR3Upscaler.fDeltaPreExposure) * 1.0;
            float _406 = _405.x;
            float _409 = 0.5 * _405.y;
            float _411 = _405.z;
            float _412 = 0.25 * _411;
            _421 = vec3(((0.25 * _406) + _409) + _412, (0.5 * _406) - (0.5 * _411), (((-0.25) * _406) + _409) - _412);
        }
        else
        {
            _421 = vec3(0.0);
        }
        vec2 _424 = _222 * cbFSR3Upscaler.fDownscaleFactor;
        ivec2 _426 = ivec2(floor(_424));
        ivec2 _432 = _426;
        _432.x = max(1, min(_426.x, (cbFSR3Upscaler.iRenderSize.x - 2)));
        ivec2 _438 = _432;
        _438.y = max(1, min(_426.y, (cbFSR3Upscaler.iRenderSize.y - 2)));
        vec2 _442 = vec2(_438) + cbFSR3Upscaler.fJitter;
        vec2 _443 = _442 - _424;
        bool _446 = _442.x > _424.x;
        ivec2 _448 = _211;
        _448.x = _446 ? (-2) : (-1);
        bool _451 = _442.y > _424.y;
        ivec2 _453 = _448;
        _453.y = _451 ? (-2) : (-1);
        vec2 _454 = vec2(_453);
        int _455 = _446 ? 3 : 0;
        int _456 = _451 ? 3 : 0;
        ivec2 _457 = ivec2(_455, _456);
        ivec2 _458 = _438 + _453;
        uvec2 _460 = uvec2(_458 + _457);
        vec3 _464 = texelFetch(SPIRV_Cross_Combinedr_input_color_jitteredSPIRV_Cross_DummySampler, ivec2(_460), int(0u)).xyz * 1.0;
        float _465 = _464.x;
        float _468 = 0.5 * _464.y;
        float _470 = _464.z;
        float _471 = 0.25 * _470;
        vec3 _479 = vec3(((0.25 * _465) + _468) + _471, (0.5 * _465) - (0.5 * _470), (((-0.25) * _465) + _468) - _471);
        int _480 = _446 ? 2 : 1;
        ivec2 _481 = ivec2(_480, _456);
        uvec2 _483 = uvec2(_458 + _481);
        vec3 _486 = texelFetch(SPIRV_Cross_Combinedr_input_color_jitteredSPIRV_Cross_DummySampler, ivec2(_483), int(0u)).xyz * 1.0;
        float _487 = _486.x;
        float _490 = 0.5 * _486.y;
        float _492 = _486.z;
        float _493 = 0.25 * _492;
        vec3 _501 = vec3(((0.25 * _487) + _490) + _493, (0.5 * _487) - (0.5 * _492), (((-0.25) * _487) + _490) - _493);
        int _502 = _446 ? 1 : 2;
        ivec2 _503 = ivec2(_502, _456);
        uvec2 _505 = uvec2(_458 + _503);
        vec3 _508 = texelFetch(SPIRV_Cross_Combinedr_input_color_jitteredSPIRV_Cross_DummySampler, ivec2(_505), int(0u)).xyz * 1.0;
        float _509 = _508.x;
        float _512 = 0.5 * _508.y;
        float _514 = _508.z;
        float _515 = 0.25 * _514;
        vec3 _523 = vec3(((0.25 * _509) + _512) + _515, (0.5 * _509) - (0.5 * _514), (((-0.25) * _509) + _512) - _515);
        int _524 = _451 ? 2 : 1;
        ivec2 _525 = ivec2(_455, _524);
        uvec2 _527 = uvec2(_458 + _525);
        vec3 _530 = texelFetch(SPIRV_Cross_Combinedr_input_color_jitteredSPIRV_Cross_DummySampler, ivec2(_527), int(0u)).xyz * 1.0;
        float _531 = _530.x;
        float _534 = 0.5 * _530.y;
        float _536 = _530.z;
        float _537 = 0.25 * _536;
        vec3 _545 = vec3(((0.25 * _531) + _534) + _537, (0.5 * _531) - (0.5 * _536), (((-0.25) * _531) + _534) - _537);
        ivec2 _546 = ivec2(_480, _524);
        uvec2 _548 = uvec2(_458 + _546);
        vec3 _551 = texelFetch(SPIRV_Cross_Combinedr_input_color_jitteredSPIRV_Cross_DummySampler, ivec2(_548), int(0u)).xyz * 1.0;
        float _552 = _551.x;
        float _555 = 0.5 * _551.y;
        float _557 = _551.z;
        float _558 = 0.25 * _557;
        vec3 _566 = vec3(((0.25 * _552) + _555) + _558, (0.5 * _552) - (0.5 * _557), (((-0.25) * _552) + _555) - _558);
        ivec2 _567 = ivec2(_502, _524);
        uvec2 _569 = uvec2(_458 + _567);
        vec3 _572 = texelFetch(SPIRV_Cross_Combinedr_input_color_jitteredSPIRV_Cross_DummySampler, ivec2(_569), int(0u)).xyz * 1.0;
        float _573 = _572.x;
        float _576 = 0.5 * _572.y;
        float _578 = _572.z;
        float _579 = 0.25 * _578;
        vec3 _587 = vec3(((0.25 * _573) + _576) + _579, (0.5 * _573) - (0.5 * _578), (((-0.25) * _573) + _576) - _579);
        int _588 = _451 ? 1 : 2;
        ivec2 _589 = ivec2(_455, _588);
        uvec2 _591 = uvec2(_458 + _589);
        vec3 _594 = texelFetch(SPIRV_Cross_Combinedr_input_color_jitteredSPIRV_Cross_DummySampler, ivec2(_591), int(0u)).xyz * 1.0;
        float _595 = _594.x;
        float _598 = 0.5 * _594.y;
        float _600 = _594.z;
        float _601 = 0.25 * _600;
        vec3 _609 = vec3(((0.25 * _595) + _598) + _601, (0.5 * _595) - (0.5 * _600), (((-0.25) * _595) + _598) - _601);
        ivec2 _610 = ivec2(_480, _588);
        uvec2 _612 = uvec2(_458 + _610);
        vec3 _615 = texelFetch(SPIRV_Cross_Combinedr_input_color_jitteredSPIRV_Cross_DummySampler, ivec2(_612), int(0u)).xyz * 1.0;
        float _616 = _615.x;
        float _619 = 0.5 * _615.y;
        float _621 = _615.z;
        float _622 = 0.25 * _621;
        vec3 _630 = vec3(((0.25 * _616) + _619) + _622, (0.5 * _616) - (0.5 * _621), (((-0.25) * _616) + _619) - _622);
        ivec2 _631 = ivec2(_502, _588);
        uvec2 _633 = uvec2(_458 + _631);
        vec3 _636 = texelFetch(SPIRV_Cross_Combinedr_input_color_jitteredSPIRV_Cross_DummySampler, ivec2(_633), int(0u)).xyz * 1.0;
        float _637 = _636.x;
        float _640 = 0.5 * _636.y;
        float _642 = _636.z;
        float _643 = 0.25 * _642;
        vec3 _651 = vec3(((0.25 * _637) + _640) + _643, (0.5 * _637) - (0.5 * _642), (((-0.25) * _637) + _640) - _643);
        vec2 _654 = _443 + (_454 + vec2(_457));
        float _659 = min(dot(_654, _654), 4.0);
        float _661 = (0.4000000059604644775390625 * _659) - 1.0;
        float _663 = (0.25 * _659) - 1.0;
        float _669 = float(all(lessThan(_460, _237))) * ((((1.5625 * _661) * _661) - 0.5625) * (_663 * _663));
        vec2 _673 = _443 + (_454 + vec2(_481));
        float _678 = min(dot(_673, _673), 4.0);
        float _680 = (0.4000000059604644775390625 * _678) - 1.0;
        float _682 = (0.25 * _678) - 1.0;
        float _688 = float(all(lessThan(_483, _237))) * ((((1.5625 * _680) * _680) - 0.5625) * (_682 * _682));
        vec2 _696 = _443 + (_454 + vec2(_503));
        float _701 = min(dot(_696, _696), 4.0);
        float _703 = (0.4000000059604644775390625 * _701) - 1.0;
        float _705 = (0.25 * _701) - 1.0;
        float _711 = float(all(lessThan(_505, _237))) * ((((1.5625 * _703) * _703) - 0.5625) * (_705 * _705));
        vec2 _719 = _443 + (_454 + vec2(_525));
        float _724 = min(dot(_719, _719), 4.0);
        float _726 = (0.4000000059604644775390625 * _724) - 1.0;
        float _728 = (0.25 * _724) - 1.0;
        float _734 = float(all(lessThan(_527, _237))) * ((((1.5625 * _726) * _726) - 0.5625) * (_728 * _728));
        vec2 _742 = _443 + (_454 + vec2(_546));
        float _747 = min(dot(_742, _742), 4.0);
        float _749 = (0.4000000059604644775390625 * _747) - 1.0;
        float _751 = (0.25 * _747) - 1.0;
        float _757 = float(all(lessThan(_548, _237))) * ((((1.5625 * _749) * _749) - 0.5625) * (_751 * _751));
        vec2 _765 = _443 + (_454 + vec2(_567));
        float _770 = min(dot(_765, _765), 4.0);
        float _772 = (0.4000000059604644775390625 * _770) - 1.0;
        float _774 = (0.25 * _770) - 1.0;
        float _780 = float(all(lessThan(_569, _237))) * ((((1.5625 * _772) * _772) - 0.5625) * (_774 * _774));
        vec2 _788 = _443 + (_454 + vec2(_589));
        float _793 = min(dot(_788, _788), 4.0);
        float _795 = (0.4000000059604644775390625 * _793) - 1.0;
        float _797 = (0.25 * _793) - 1.0;
        float _803 = float(all(lessThan(_591, _237))) * ((((1.5625 * _795) * _795) - 0.5625) * (_797 * _797));
        vec2 _811 = _443 + (_454 + vec2(_610));
        float _816 = min(dot(_811, _811), 4.0);
        float _818 = (0.4000000059604644775390625 * _816) - 1.0;
        float _820 = (0.25 * _816) - 1.0;
        float _826 = float(all(lessThan(_612, _237))) * ((((1.5625 * _818) * _818) - 0.5625) * (_820 * _820));
        vec2 _834 = _443 + (_454 + vec2(_631));
        float _839 = min(dot(_834, _834), 4.0);
        float _841 = (0.4000000059604644775390625 * _839) - 1.0;
        float _843 = (0.25 * _839) - 1.0;
        float _849 = float(all(lessThan(_633, _237))) * ((((1.5625 * _841) * _841) - 0.5625) * (_843 * _843));
        vec3 _851 = ((((((((_479 * _669) + (_501 * _688)) + (_523 * _711)) + (_545 * _734)) + (_566 * _757)) + (_587 * _780)) + (_609 * _803)) + (_630 * _826)) + (_651 * _849);
        float _852 = (((((((_669 + _688) + _711) + _734) + _757) + _780) + _803) + _826) + _849;
        vec3 _853 = min(min(min(min(min(min(min(min(_479, _501), _523), _545), _566), _587), _609), _630), _651);
        vec3 _854 = max(max(max(max(max(max(max(max(_479, _501), _523), _545), _566), _587), _609), _630), _651);
        float _857 = _852 * float(_852 > 6.099999882280826568603515625e-05);
        vec3 _864;
        if (_857 > 6.099999882280826568603515625e-05)
        {
            _864 = clamp(_851 / vec3(_857), _853, _854);
        }
        else
        {
            _864 = _851;
        }
        vec3 _890;
        if (any(greaterThan(_853, _421)) || any(greaterThan(_421, _854)))
        {
            vec3 _872 = clamp(_421, _853, _854);
            vec3 _874 = abs(_421 - _872);
            float _875 = _874.x;
            float _876 = _874.y;
            float _878 = _874.z;
            vec3 _889;
            if (any(greaterThan(vec3((_875 + _876) - _878, _875 + _878, (_875 - _876) - _878), vec3(0.00999999977648258209228515625))))
            {
                _889 = mix(_872, _421, vec3(0.00999999977648258209228515625));
            }
            else
            {
                _889 = _421;
            }
            _890 = _889;
        }
        else
        {
            _890 = _421;
        }
        vec3 _893 = mix(_890, _864, vec3(_335 ? 0.0999999940395355224609375 : 1.0));
        float _894 = _893.x;
        float _895 = _893.y;
        float _897 = _893.z;
        imageStore(rw_internal_upscaled_color, ivec2(uvec2(ivec3(gl_GlobalInvocationID).xy)), vec4(max(vec3((_894 + _895) - _897, _894 + _897, (_894 - _895) - _897), vec3(0.0)), 1.0));
    }
}

