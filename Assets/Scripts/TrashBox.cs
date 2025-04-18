using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;
using UnityEngine.Serialization;

public class TrashBox : MonoBehaviour
{
    [SerializeField] private string TagToCompare;
    [SerializeField] private Material GreenMaterial;
    [SerializeField] private Material RedMaterial;
    [SerializeField] private List<MeshRenderer> MeshRenderers;
    [SerializeField] private AudioClip Audio;
    [SerializeField] private AudioClip Error;
    [SerializeField] private AudioSource AudioSource;
    

    private List<string> _tags;
    
    private Material _defaultMaterial;

    private void Start()
    {
        _tags = new List<string>() { "Glass", "Metal", "Other", "Paper" };
        _defaultMaterial = MeshRenderers[0].material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagToCompare))
        {
            Destroy(other.gameObject);
            foreach (var mr in MeshRenderers)
            {
                mr.material = GreenMaterial;    
                AudioSource.PlayOneShot(Audio);
            }
            
            StartCoroutine(returnMaterialBack());
        }
        else if(_tags.Contains(other.tag))
        {

            Destroy(other.gameObject);
            foreach (var mr in MeshRenderers)
            {
                mr.material = RedMaterial;    
                AudioSource.PlayOneShot(Error);
            }
            StartCoroutine(returnMaterialBack());
        }
    }

    IEnumerator returnMaterialBack()
    {
        yield return new WaitForSeconds(1f);
        foreach (var mr in MeshRenderers)
        {
            mr.material = _defaultMaterial;    
        }
    }
}
