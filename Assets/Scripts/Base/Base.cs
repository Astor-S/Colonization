using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour, IDestroyable<Base>
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private List<Unit> _units;
    [SerializeField] private SpawnerUnits _spawnerUnits;
    [SerializeField] private float _scanDelayTime = 1f;
    [SerializeField] private int _minCountUnits = 1;

    private ResourcesDatabase _resourcesDatabase = new();
    private WaitForSeconds _scanDelay;
    
    private int _resourceCount;
    private int _amountResourcesForUnitCreate = 3;
    private int _amountResourcesForBaseCreate = 5;

    private bool _IsCreateUnit = true;

    public event Action<Base> RequestedCreationBase;
    public event Action<Base> Destroyed;

    public Flag Flag { get; private set; }

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
            _resourcesDatabase.RemoveReservation(resource);
        }
    }

    public void PlaceFlag(Flag flag)
    {
        Flag = flag;
        flag.transform.SetParent(transform);

        _IsCreateUnit = false;
    }

    public void MoveFlag(Flag newFlag)
    {
        if (Flag != null)
            Flag.Destroy();

        Flag = newFlag;
        newFlag.transform.SetParent(transform);
    }

    public Unit GetFreeUnit()
    {
        foreach (Unit unit in _units)
        {
            if (unit.IsBusy == false)
            {
                return unit;
            }
        }

        return null;
    }

    public void RemoveUnit(Unit unitToRemove)
    {
        _units.Remove(unitToRemove);
    }

    public void SpendResourcesCreatingBase()
    {
        _resourceCount -= _amountResourcesForBaseCreate;
    }

    public void ActivateBasicBehavior()
    {
        _IsCreateUnit = true;
    }

    private void ReceiveResource()
    {
        _resourceCount++;
        Create();
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

        List<Resource> freeResources = _resourcesDatabase.GetFreeResources(_scanner.Scan()).ToList();

        if (freeUnits.Count > 0 && freeResources.Count > 0)
        {
            foreach (Unit unit in freeUnits)
            {
                Resource resource = freeResources[0];
                _resourcesDatabase.ReserveResources(resource);
                unit.SendToResource(resource);
                freeResources.RemoveAt(0); 
                
                if (freeResources.Count == 0)
                    break; 
            }
        }
    }

    private void Create()
    {
        if (_IsCreateUnit == true)
            CreateUnit();
        else 
            RequestCreateBase();  
    }

    private void CreateUnit()
    {
        if (_IsCreateUnit == true && _resourceCount >= _amountResourcesForUnitCreate)
        {
            Unit newUnit = _spawnerUnits.Spawn(transform.position);
            _units.Add(newUnit);
            _resourceCount -= _amountResourcesForUnitCreate;
        }
    }

    private void RequestCreateBase()
    {
        if (_units.Count > _minCountUnits && _IsCreateUnit == false && _resourceCount >= _amountResourcesForBaseCreate)     
            RequestedCreationBase?.Invoke(this);              
    }
}