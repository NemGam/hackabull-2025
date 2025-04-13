using System.Collections;
using UnityEngine;

public class AlignSelf : MonoBehaviour
{
    [SerializeField] private float valueY;
    [SerializeField] private bool alignRotation = true;
    

    private Vector3 _targetPosition;
    void Start()
    {
        if(alignRotation)
            transform.rotation = Quaternion.identity;
        
        Vector3 pos = transform.position;
        pos.y = valueY;
        
        _targetPosition = pos;

        StartCoroutine(Position());
    }

    IEnumerator Position()
    {
        int iter = 0;
        while (transform.position != _targetPosition && iter < 10)
        {
            transform.position = _targetPosition;
            yield return new WaitForSeconds(0.1f);
            iter += 1;
        }
        
    }
    
}