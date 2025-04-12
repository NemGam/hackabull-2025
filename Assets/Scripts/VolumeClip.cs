using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class VolumeClip : MonoBehaviour
{
    [Tooltip("Material using your Shader Graph with the Custom Function node")]
    public Material maskMaterial;
    
        [Tooltip("List of sphere Transforms")]
        public List<Transform> sphereTransforms;
    
        void Update()
        {
            
            // clamp to our MAX_SPHERES (64)
            int count = Mathf.Min(sphereTransforms.Count, 64);
            var data  = new Vector4[count];
    
            for (int i = 0; i < count; i++)
            {
                Vector3 p = sphereTransforms[i].position;
                float   r = sphereTransforms[i].localScale.x / 2;
                data[i]    = new Vector4(p.x, p.y, p.z, r);
            }
    
            // feed the HLSL globals
            maskMaterial.SetVectorArray("_Spheres",    data);
            maskMaterial.SetInt       ("_SphereNumber", count);
        }
}
