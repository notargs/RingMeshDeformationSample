Shader "Polar/Unlit"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _PolarBlend ("PolarBlend", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"
            #define PI 3.1415926535

            struct appdata
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
            UNITY_INSTANCING_BUFFER_END(Props)

            float _PolarBlend;

            v2f vert (appdata v)
            {
                v2f o;
                
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                
                float4 worldPos = mul(UNITY_MATRIX_M, float4(v.vertex.xyz, 1.0));
                float angle = worldPos.x * PI * 2 * _PolarBlend;
                worldPos.xy = worldPos.xy * (1 - _PolarBlend) + float2(sin(angle), cos(angle)) * (worldPos.y * _PolarBlend);
                o.vertex = mul(UNITY_MATRIX_VP, worldPos);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                return UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
            }
            ENDCG
        }
    }
}
