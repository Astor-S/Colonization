using System;
using UnityEngine;

public class Flag : MonoBehaviour, IDestroyable<Flag>
{
    public event Action<Flag> Destroyed;
}