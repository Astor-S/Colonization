using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private Picker _picker;
    [SerializeField] private Base _base;
    
    private bool _isBusy = false;

    public bool IsBusy => _isBusy;

    public void SendToResource(Resource resource)
    {
        _isBusy = true;
        _mover.MoveTo(resource.transform);
        StartCoroutine(CollectResource(resource));
    }

    private IEnumerator CollectResource(Resource resource)
    {
        while ((transform.position - resource.transform.position).sqrMagnitude > _picker.PickUpDistance)
        {
            yield return null;
        }

        _picker.PickUp(resource);
        _mover.MoveTo(_base.transform);

        while ((transform.position - _base.transform.position).sqrMagnitude > _picker.PickUpDistance)
        {
            yield return null;
        }

        _picker.Release();
        _isBusy = false; 
    }
}