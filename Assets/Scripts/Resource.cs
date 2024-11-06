using System;
using UnityEngine;

public class Resource : MonoBehaviour, IDestroyable<Resource>
{
    public event Action<Resource> Destroyed;

    public void Destroy()
    {
        Destroyed?.Invoke(this);
    }
}