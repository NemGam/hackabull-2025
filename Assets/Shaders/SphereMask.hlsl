// SphereMask.hlsl

// maximum spheres we support
#define MAX_SPHERES 64

// global array: xyz = center, w = radius
float4 _Spheres[MAX_SPHERES];   // xyz = center, w = radius
int   _SphereNumber;


float SampleSphereMask_float (float3 worldPos, out float mask)
{
    // Union of all spheres: bail out as soon as we hit one
    for (int i = 0; i < _SphereNumber; i++)
    {
        float4 sp = _Spheres[i];
        float3  d  = worldPos - sp.xyz;
        // squared‐distance test
        if (dot(d, d) < sp.w * sp.w)
        {
            mask =  1.0f;
            return 1.0f;
        }
            
    }
    mask = 0.0f;
    return 0.0f;
}
