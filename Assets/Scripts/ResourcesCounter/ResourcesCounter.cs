using TMPro;
using UnityEngine;

public class ResourcesCounter : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private TextMeshProUGUI _resourcesCountText;

    private void Start()
    {
        _base.ResourceCountChanged += UpdateResourceCount;
        UpdateResourceCount(); 
    }

    private void UpdateResourceCount()
    {
        _resourcesCountText.text = _base.ResourceCount.ToString();
    }
}