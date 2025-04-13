using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class TrashBox : MonoBehaviour
{
    [SerializeField] private string TagToCompare;
    [SerializeField] private Material GreenMaterial;
    [SerializeField] private Material RedMaterial;
    

    private List<string> _tags;
    private MeshRenderer _meshRenderer;
    private Material _defaultMaterial;

    private void Start()
    {
        _tags = new List<string>() { "Glass", "Metal", "Other", "Paper" };
        _meshRenderer = GetComponent<MeshRenderer>();
        _defaultMaterial = _meshRenderer.material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagToCompare))
        {
            Destroy(other.gameObject);
            _meshRenderer.material = GreenMaterial;
            StartCoroutine(returnMaterialBack());
        }
        else if(_tags.Contains(other.tag))
        {
            Destroy(other.gameObject);
            _meshRenderer.material = RedMaterial;
            StartCoroutine(returnMaterialBack());
        }
    }

    IEnumerator returnMaterialBack()
    {
        yield return new WaitForSeconds(1f);
        _meshRenderer.material = _defaultMaterial;
    }
}
