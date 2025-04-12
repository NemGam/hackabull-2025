using UnityEngine;
[ExecuteAlways]
public class VolumeClip : MonoBehaviour
{
    public Transform sphere;

    void Update()
    {
        Shader.SetGlobalVector("_SphereCenter", sphere.position);
        Shader.SetGlobalFloat ("_SphereRadius", sphere.localScale.x / 2);
        
        Debug.Log(Shader.GetGlobalVector("_SphereCenter"));
    }
}
