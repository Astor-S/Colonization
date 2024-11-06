using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private List<Unit> _units;
    [SerializeField] private float _scanDelayTime = 1f;

    private ResourcesDatabase _resourcesDatabase = new();
    private WaitForSeconds _scanDelay;
    private int _resourceCount;

    private void Awake()
    {
        _scanDelay = new WaitForSeconds(_scanDelayTime);
    }

    private void OnEnable()
    {
        StartCoroutine(MineResources());
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out Resource resource))
        {
            resource.Destroy();
            ReceiveResource();
        }
    }

    private void ReceiveResource()
    {
        _resourceCount++;
        Debug.Log($"Количество ресурсов на базе: {_resourceCount}");
    }

    private IEnumerator MineResources()
    {
        while(enabled)
        {
            yield return _scanDelay;

            SendUnitsForResources();
        }
    }

    private void SendUnitsForResources()
    {
        List<Unit> freeUnits = _units.Where(unit => unit.IsBusy == false).ToList();

        List<Resource> resources = _scanner.Scan().Where(resource => resource.IsBusy == false).ToList();

        if (freeUnits.Count > 0 && resources.Count > 0)
        {
            foreach (Unit unit in freeUnits)
            {
                Resource resource = resources[0]; 
                resource.MarkBusy(); 
                unit.SendToResource(resources[0]);
                resources.RemoveAt(0); 
                
                if (resources.Count == 0)
                    break; 
            }
        }
    }
}