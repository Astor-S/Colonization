using UnityEngine;

public class Picker : MonoBehaviour
{
    [SerializeField] private float _pickUpDistance = 0.1f;
    [SerializeField] private float _pickUpHeight = 2f;

    private Resource _heldResource;

    public float PickUpDistance => _pickUpDistance;

    public void PickUp(Resource resource)
    {
        if (_heldResource != null)
            return;
        
        _heldResource = resource;

        resource.transform.SetParent(transform);
        resource.transform.localPosition = Vector3.up * _pickUpHeight; 
    }

    public void Release()
    {
        if (_heldResource != null)
        {
            _heldResource.transform.parent = null; 
            _heldResource = null;
        }
    }
}