using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class DelayedActivation : MonoBehaviour
{
    [SerializeField] private GameObject objj;
    [SerializeField] private float ActivationDelay;
    void Start()
    {
        StartCoroutine(LaunchingTheObject());
    }

    IEnumerator LaunchingTheObject()
    {
        yield return new WaitForSeconds(ActivationDelay);
        objj.SetActive(true);
    }
}
