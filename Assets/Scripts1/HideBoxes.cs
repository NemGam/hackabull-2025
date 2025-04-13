using System;
using UnityEngine;


public class HideBoxes : MonoBehaviour
{
    [SerializeField] private GameObject Boxes;
    [SerializeField] private float yHeight;
    [SerializeField] private Transform CenterEye;
    
    private void Update()
    {
        
        Boxes.SetActive(transform.position.y >= yHeight);
        
        if(Boxes.activeSelf)
            Boxes.transform.LookAt(CenterEye);
        
    }
    
}
