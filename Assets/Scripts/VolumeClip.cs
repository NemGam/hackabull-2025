using System;
using System.Collections.Generic;
using UnityEngine;


[ExecuteAlways]
public class VolumeClip : MonoBehaviour
{
    public static VolumeClip Instance;

    [SerializeField, Tooltip("Dummy sphere, DO NOT DELETE. REQUIRED FOR SHADER TO WORK")] private Transform dummySphere;
    
    [SerializeField, Tooltip("List of sphere Transforms")]
    private HashSet<Transform> _sphereTransforms = new HashSet<Transform>();

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
        _sphereTransforms.Add(self);
    }

    public void Unregister(Transform self)
    {
        _sphereTransforms.Remove(self);
    }
    
    private void Update()
    {
        
        // clamp to our MAX_SPHERES (64)
        int count = Mathf.Min(_sphereTransforms.Count, 64);
        var data  = new Vector4[64];
        int i = 1;

        var position = dummySphere.position;
        data[0] = new Vector4(position.x, position.y, position.z,
            dummySphere.localScale.x / 2);
        Debug.Log(_sphereTransforms.Count);
        foreach (var trans in _sphereTransforms)
        {
            Vector3 p = trans.position;
            float r = trans.localScale.x / 2;
            data[i] = new Vector4(p.x, p.y, p.z, r);
            i++;
        }
        
        // feed the HLSL globals
        Shader.SetGlobalVectorArray("_Spheres", data);
        Shader.SetGlobalInt("_SphereNumber", _sphereTransforms.Count + 1);
    }
}
