using UnityEngine;

public class PerformanceTest : MonoBehaviour
{
    public Transform clipper;
    void Update()
    {
        clipper.localScale = Vector3.one * (2 * Mathf.Sin(Time.time));
    }
}
