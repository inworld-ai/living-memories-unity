Shader "Inworld/UI/RotatingImage"
{
Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        _RotationSpeed ("Rotation Speed", Float) = 1.0
        [Toggle] _UseAlpha ("Use Texture Alpha", Float) = 1
    }
    SubShader
    {
        Tags { 
            "RenderType"="Transparent" 
            "Queue"="Transparent"
            "IgnoreProjector"="True"
        }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature _USEALPHA_ON
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _RotationSpeed;
            float _UseAlpha;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color * _Color;

                float2 center = float2(0.5, 0.5);

                float angle = _Time.y * _RotationSpeed;

                float sinAngle, cosAngle;
                sincos(angle, sinAngle, cosAngle);

                float2 uv = TRANSFORM_TEX(v.uv, _MainTex);
                uv -= center;

                float2 rotatedUV;
                rotatedUV.x = uv.x * cosAngle - uv.y * sinAngle;
                rotatedUV.y = uv.x * sinAngle + uv.y * cosAngle;

                rotatedUV += center;
                
                o.uv = rotatedUV;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;

                #if _USEALPHA_ON
                    col.a = tex2D(_MainTex, i.uv).a * i.color.a;
                #else
                    col.a = i.color.a;
                #endif
                
                return col;
            }
            ENDCG
        }
    }
}
