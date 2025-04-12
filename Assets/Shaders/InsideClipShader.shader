Shader "Custom/InsideClipShader"
{
    Properties
    {
        _MainTex       ("Albedo", 2D)     = "white" {}
//        _SphereCenter  ("Sphere Center", Vector) = (0,0,0,0)
//        _SphereRadius  ("Sphere Radius", Float)  = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4   _SphereCenter;
            float    _SphereRadius;

            struct v2f
            {
                float4 pos      : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float2 uv       : TEXCOORD1;
            };

            v2f vert(appdata_full v)
            {
                v2f o;
                o.pos      = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.uv       = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float d = distance(i.worldPos, _SphereCenter.xyz);
                // discard anything **outside** the sphere
                clip(_SphereRadius - d);

                return tex2D(_MainTex, i.uv);
            }
            ENDCG
        }
    }
}
