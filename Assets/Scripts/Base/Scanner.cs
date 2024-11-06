using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _radius = 50f;
    [SerializeField] private LayerMask _resourcesLayer;

    public IEnumerable<Resource> Scan()
    {
        List<Resource> resources = new List<Resource>();

        foreach (Collider collider in Physics.OverlapSphere(transform.position, _radius, _resourcesLayer))
        {
            if (collider.TryGetComponent(out Resource resource))
                resources.Add(resource);
        }

        return resources;
    }
}