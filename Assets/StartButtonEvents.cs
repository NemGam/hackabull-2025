using System;
using UnityEngine;

public class StartButtonEvents : MonoBehaviour
{
    [SerializeField] float smoothSpeed = 5f;
    
    private Vector3 originalScale;
    private Vector3 enlargeScale;

    private void Start()
    {
        originalScale = transform.localScale;
        enlargeScale = originalScale * 1.3f;
    }

    public void HoverEnlarge()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, enlargeScale, Time.deltaTime * smoothSpeed);
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void UnhoverEnlarge()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * smoothSpeed);
    }

    public void OnSelect()
    {
        GetComponent<Renderer>().material.color = Color.blue;
    }
}
