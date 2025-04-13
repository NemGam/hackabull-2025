using System;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Trash : MonoBehaviour
{
    [SerializeField] private Transform pollutionBubblePrefab;
    private PollutionBubble _createdPollutionBubble;
    [SerializeField, Tooltip("Time before the pollution starts to spread")] private float gracePeriod;
    private float _timer;
    [SerializeField]
    private Rigidbody rigidbody;
    
    private int _groundLayer; 
    
    private void Start()
    {
        _timer = gracePeriod;
        _groundLayer = LayerMask.NameToLayer("Ground");
        enabled = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_createdPollutionBubble) return;
        if (other.gameObject.layer != _groundLayer) return;
        enabled = true;
    }
    
    private void FixedUpdate()
    {
        if (!rigidbody.IsSleeping()) return;
        
        _timer -= Time.fixedDeltaTime;

        if (_timer > 0) return;

        //Prevent it from spamming pollution bubbles
        enabled = false;
        _createdPollutionBubble = 
            Instantiate(pollutionBubblePrefab, transform.position, Quaternion.identity).GetComponent<PollutionBubble>();
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.layer != _groundLayer) return;
        enabled = false;
        _timer = gracePeriod;
        if (_createdPollutionBubble) _createdPollutionBubble.StartShrinkage();
        _createdPollutionBubble = null;
    }
}
