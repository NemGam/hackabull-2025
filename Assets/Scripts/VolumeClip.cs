using System;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public class VolumeClip : MonoBehaviour
{
    public static VolumeClip Instance;
    
    [SerializeField, Tooltip("Dummy sphere, DO NOT DELETE. REQUIRED FOR SHADER TO WORK")] private Transform dummySphere;
    
    [SerializeField, Tooltip("List of sphere Transforms")]
    public readonly HashSet<Transform> SphereTransforms = new HashSet<Transform>();

    [SerializeField] private float maxExpansion;
    
    private void Awake()
    {
        
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this; 
        }

        // DontDestroyOnLoad(gameObject);
    }
    
    public void Register(Transform self)
    {
        SphereTransforms.Add(self);
    }

    public void Unregister(Transform self)
    {
        SphereTransforms.Remove(self);
    }
    
    private void Update()
    {
        
        // clamp to our MAX_SPHERES (64)
        int count = Mathf.Min(SphereTransforms.Count, 64);
        var data  = new Vector4[64];
        int i = 1;

        var position = dummySphere.position;
        data[0] = new Vector4(position.x, position.y, position.z,
            dummySphere.localScale.x / 2);

        foreach (var trans in SphereTransforms)
        {
            Vector3 p = trans.transform.position;
            float r = trans.transform.localScale.x / 2;
            data[i] = new Vector4(p.x, p.y, p.z, r);
            i++;
        }
        
        // feed the HLSL globals
        Shader.SetGlobalVectorArray("_Spheres", data);
        Shader.SetGlobalInt("_SphereNumber", SphereTransforms.Count + 1);
    }
}
