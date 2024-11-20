using TMPro;
using UnityEngine;

public class ResourcesCounter : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private TextMeshProUGUI _resourcesCountText;

    private void OnEnable()
    {
        _base.ResourceCountChanged += UpdateResourceCount;
        UpdateResourceCount();
    }

    private void OnDisable()
    {
        _base.ResourceCountChanged -= UpdateResourceCount;
    }

    private void UpdateResourceCount()
    {
        _resourcesCountText.text = _base.ResourceCount.ToString();
    }
}