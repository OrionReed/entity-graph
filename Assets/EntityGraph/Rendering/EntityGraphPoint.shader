Shader "OrionReed/EntityGraphPoint" 
{
     Properties
    {
        _Scale ("Fresnel Scale", Range(0,10)) = 5
        _Power ("Fresnel Power", Range(0,10)) = 1
    }
    SubShader {
        Tags { "RenderType" = "Opaque" }

        Pass {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t 
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
            };

            struct v2f 
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float R : FRESNEL;
            }; 

            struct PointProperties 
            {
                float4x4 mat;
                float4 color;
            };

            fixed _Scale;
            fixed _Power;

            StructuredBuffer<PointProperties> _Properties;

            v2f vert(appdata_t i, float3 normal : NORMAL, uint instanceID: SV_InstanceID) 
            {
                v2f o;

                float4 pos = mul(_Properties[instanceID].mat, i.vertex);
	            float3 posWorld = mul(unity_ObjectToWorld, i.vertex).xyz;
                float3 worldNorm = normalize(mul(UNITY_MATRIX_MV, normal));
	            float3 I = normalize(posWorld - _WorldSpaceCameraPos.xyz);
                o.vertex = UnityObjectToClipPos(pos);
                o.color = _Properties[instanceID].color;
	            o.R = _Scale * pow(1.0 + dot(I, worldNorm), _Power);
                
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                return lerp(i.color,i.color/1.1, i.R);
            }

            ENDCG            
        }
    }
}

