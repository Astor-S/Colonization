using System;
using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour, IDestroyable<Unit>
{
    [SerializeField] private Mover _mover;
    [SerializeField] private Picker _picker;
    [SerializeField] private Base _base;
    
    private bool _isBusy = false;

    public bool IsBusy => _isBusy;

    public event Action<Unit> Destroyed;

    public void SendToResource(Resource resource)
    {
        _isBusy = true;
        _mover.MoveTo(resource.transform);
        StartCoroutine(CollectResource(resource));
    }

    public void Init(Base @base)
    {
        _base = @base;
    }

    public void SendToFlag(Flag flag, Action onFlagReached)
    {
        _isBusy = true;
        _mover.MoveTo(flag.transform);

        StartCoroutine(MoveToFlag(flag, onFlagReached));
    }

    public void ChangeOwner(Base newBase)
    {
        _base = newBase;
    }

    private IEnumerator MoveToFlag(Flag flag, Action onFlagReached)
    {
        yield return new WaitUntil(() =>
            (transform.position - flag.transform.position).sqrMagnitude <= _picker.PickUpDistance);

        _isBusy = false;

        onFlagReached?.Invoke();
    }

    private IEnumerator CollectResource(Resource resource)
    {
        yield return new WaitUntil(() =>
            (transform.position - resource.transform.position).sqrMagnitude <= _picker.PickUpDistance);

        _picker.PickUp(resource);
        _mover.MoveTo(_base.transform);

        yield return new WaitUntil(() =>
            (transform.position - _base.transform.position).sqrMagnitude <= _picker.PickUpDistance);

        _picker.Release();
        _isBusy = false; 
    }
}