Shader "Unlit/TimeVaryingGlow"
{
    Properties
    {
        // Basic glow properties
        _Radius     ("Radius (0~1)", Range(0.0,1.0)) = 0.25
        _Softness   ("Edge Softness", Range(0.001, 1.0)) = 0.2
        _Intensity  ("Intensity", Range(0.0, 5.0)) = 1.5
        _Speed      ("Pulse Speed", Range(0.0, 2.0)) = 0.25
        _PulseAmp   ("Pulse Amplitude", Range(0.0, 0.2)) = 0.05
        _NoiseScale ("Noise Scale", Range(0.0, 10.0)) = 2.0
        _Center     ("Center (UV)", Vector) = (0.5, 0.5, 0, 0)

        // Dual color gradient
        _ColorA     ("Color A", Color) = (1,0.85,0.5,1)
        _ColorB     ("Color B", Color) = (0.5,0.8,1,1)

        // Gradient animation controls
        _GradRotateSpeed ("Linear Grad Rotate Speed", Range(0, 4)) = 0.6  // Linear gradient direction rotation speed
        _GradModeSpeed   ("Mode Lerp Speed", Range(0, 4)) = 0.5          // Speed of switching between radial/linear modes
        _GradShiftAmp    ("Gradient Shift Amp", Range(0, 1)) = 0.2       // Gradient offset amplitude (color center drift over time)

        // Mask texture (controls glow region, R channel)
        _MaskTex   ("Glow Mask (R)", 2D) = "white" {}
        _MaskPower ("Mask Power", Range(0.0, 4.0)) = 1.0
        _MaskBlend ("Mask Blend", Range(0.0, 1.0)) = 1.0  // 1=fully controlled by mask, 0=no mask effect
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
            float  _Radius, _Softness, _Intensity, _Speed, _PulseAmp, _NoiseScale;
            float4 _Center;
            float4 _ColorA, _ColorB;
            float  _GradRotateSpeed, _GradModeSpeed, _GradShiftAmp;
            float  _MaskPower, _MaskBlend;
            CBUFFER_END

            TEXTURE2D(_MaskTex); SAMPLER(sampler_MaskTex);
            float4 _MaskTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = TransformObjectToHClip(v.vertex.xyz);
                o.uv  = v.uv;
                return o;
            }

            // Lightweight value noise
            float hash21(float2 p) {
                p = frac(p * float2(123.34, 345.45));
                p += dot(p, p + 34.345);
                return frac(p.x * p.y);
            }
            float noise2(float2 p) {
                float2 i = floor(p);
                float2 f = frac(p);
                float a = hash21(i);
                float b = hash21(i + float2(1,0));
                float c = hash21(i + float2(0,1));
                float d = hash21(i + float2(1,1));
                float2 u = f*f*(3.0 - 2.0*f);
                return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 center = _Center.xy;
                float2 p = i.uv - center;
                float  r = length(p);

                // Time parameter
                float t = _Time.y * _Speed;

                // Radius breathing animation with subtle noise
                float n = (noise2(i.uv * _NoiseScale + t) - 0.5) * 0.1;
                float radius = _Radius + _PulseAmp * sin(t * 6.28318) + n;

                // Base soft-edge glow (fade out from center)
                float soft = max(0.001, _Softness);
                float glow = 1.0 - smoothstep(radius, radius + soft, r);

                // Mask limitation (R channel)
                float2 uvMask = i.uv * _MaskTex_ST.xy + _MaskTex_ST.zw;
                float maskR = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, uvMask).r;
                // Optional power adjustment
                float mask = pow(saturate(maskR), _MaskPower);
                // Blend with original glow (_MaskBlend controls mask influence weight)
                glow = lerp(glow, glow * mask, _MaskBlend);

                // Dual color gradient (time-varying)
                // Gradient mode: radial vs linear (oscillates between the two over time)
                float modeLerp = 0.5 + 0.5 * sin(_Time.y * _GradModeSpeed * 6.28318); // 0~1

                // 1) Radial: transition based on r (closer to center = closer to ColorA)
                float g_rad = saturate(1.0 - smoothstep(0.0, radius + soft, r));
                // Add subtle drift to color center over time
                g_rad = saturate(g_rad + _GradShiftAmp * sin(_Time.y * 1.3));

                // 2) Linear: based on projection in a rotating direction
                float ang = _Time.y * _GradRotateSpeed * 6.28318;
                float2 dir = float2(cos(ang), sin(ang));        // Linear gradient direction
                // Project p onto dir and normalize to 0~1
                float proj = dot(p, dir) / max(radius + soft, 1e-4);
                float g_lin = saturate(proj * 0.5 + 0.5);
                g_lin = saturate(g_lin + _GradShiftAmp * cos(_Time.y * 1.1));

                // Interpolate between the two modes
                float g = lerp(g_rad, g_lin, modeLerp);

                // Final color blending
                float3 col = lerp(_ColorA.rgb, _ColorB.rgb, g);

                // Output (alpha determined by glow)
                float a = saturate(glow);
                float3 c = col * (_Intensity * glow);

                return float4(c, a);
            }
            ENDHLSL
        }
    }
    FallBack Off
}
