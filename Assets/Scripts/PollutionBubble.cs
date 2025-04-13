using System;
using UnityEngine;

public class PollutionBubble : MonoBehaviour
{
    [SerializeField] private float expansionRate, shrinkRate;
    private float _currentExpansion = 0.001f;
    private bool _isExpanding;

    private void Start()
    {
        VolumeClip.Instance.Register(transform);
        StartExpansion();
    }

    private void Update()
    {
        if (_isExpanding)
        {
            _currentExpansion += expansionRate * Time.deltaTime;
            transform.localScale = Vector3.one * _currentExpansion;
        }
        else
        {
            _currentExpansion -= shrinkRate * Time.deltaTime;
            transform.localScale = Vector3.one * _currentExpansion; 
            if (_currentExpansion <= 0f) Destroy(gameObject);
        }
        VolumeClip.Instance.ChangeOverallExpansion(_currentExpansion);
    }

    private void OnDisable()
    {
        VolumeClip.Instance.Unregister(transform);
    }

    public void StartExpansion()
    {
        _isExpanding = true;
    }

    public void StartShrinkage()
    {
        _isExpanding = false;
    }
}
