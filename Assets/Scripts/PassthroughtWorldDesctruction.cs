using System.Collections;
using UnityEngine;

public class PassthroughtWorldDesctruction : MonoBehaviour
{
    public static PassthroughtWorldDesctruction Instance { get; private set; }

    [SerializeField] private Material TreeMaterial;
    [SerializeField] private Material CloudMaterial;

    [SerializeField]
    private Color _defaultTreeColor, _defaultCloudColor;

    private bool _worldOnFire = false;
    public float WorldState { get; private set; } = 0f;

    
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
        TreeMaterial.color = _defaultTreeColor;
        CloudMaterial.color = _defaultCloudColor;
    }

    public void UpdateWorldState(float value)
    {
        value = Mathf.Clamp01(value);
        WorldState = value;
        float darkness = 1f - value; 

        Color newCloudColor = new Color(
            _defaultCloudColor.r * darkness,
            _defaultCloudColor.g * darkness,
            _defaultCloudColor.b * darkness);
        CloudMaterial.color = newCloudColor;
        
        TreeMaterial.color = new Color(
            _defaultTreeColor.r * darkness,
            _defaultTreeColor.g * darkness,
            _defaultTreeColor.b * darkness);

        if (value >= 0.7f && !_worldOnFire)
        {
            _worldOnFire = true;
        }
    }

    public void AddToWorldState(float value)
    {
        WorldState += value;
        UpdateWorldState(WorldState);
    }

}
