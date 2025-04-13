using System.Collections;
using UnityEngine;

public class AlignSelf : MonoBehaviour
{
    [SerializeField] private float valueY;

    private Vector3 _targetPosition;
    void Start()
    {
        transform.rotation = Quaternion.identity;
        
        Vector3 pos = transform.position;
        pos.y = valueY;
        
        _targetPosition = pos;

        StartCoroutine(Position());
    }

    IEnumerator Position()
    {
        while (transform.position != _targetPosition)
        {
            transform.position = _targetPosition;
            yield return new WaitForSeconds(0.1f);
        }
        
    }
    
}