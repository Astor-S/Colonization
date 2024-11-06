using System;
using UnityEngine;

public class Resource : MonoBehaviour, IDestroyable<Resource>
{
    public event Action<Resource> Destroyed;

    public bool IsBusy { get; private set; }

    public void MarkBusy()
    {
        IsBusy = true;
    }

    public void Destroy()
    {
        Destroyed?.Invoke(this);
    }
}