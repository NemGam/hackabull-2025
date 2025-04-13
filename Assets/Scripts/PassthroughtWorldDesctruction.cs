using System.Collections;
using UnityEngine;

public class PassthroughtWorldDesctruction : MonoBehaviour
{
    public static PassthroughtWorldDesctruction Instance { get; private set; }

    [SerializeField] private Material TreeMaterial;
    [SerializeField] private Material CloudMaterial;

    private Color _defaultTreeColor;
    private Color _defaultCloudColor;

    private bool _worldOnFire = false;
    private float _worldState = 0f;
    
    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Start()
    {
        _defaultTreeColor = TreeMaterial.color;
        _defaultCloudColor = CloudMaterial.color;
    }

    private void UpdateWorldState(float value)
    {
        value = Mathf.Clamp01(value); 

        float darkness = 1f - value; 

        Color newCloudColor = new Color(
            _defaultCloudColor.r * darkness,
            _defaultCloudColor.g * darkness,
            _defaultCloudColor.b * darkness);
        CloudMaterial.color = newCloudColor;

        Color newTreeColor = new Color(
            _defaultTreeColor.r * darkness,
            _defaultTreeColor.g * darkness,
            _defaultTreeColor.b * darkness);
        TreeMaterial.color = newTreeColor;

        if (value >= 0.7f && !_worldOnFire)
        {
            _worldOnFire = true;
        }
    }

    public void AddToWorldState(float value)
    {
        _worldState += value;
        UpdateWorldState(_worldState);
    }

}
