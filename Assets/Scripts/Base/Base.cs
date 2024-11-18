using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    [SerializeField] private Flag _flag;
    [SerializeField] private List<Unit> _units;
    [SerializeField] private SpawnerUnits _spawnerUnits;
    [SerializeField] private float _scanDelayTime = 1f;
    [SerializeField] private int _minCountUnitsForCreateBase = 2;

    private ResourcesDatabase _resourcesDatabase;
    private WaitForSeconds _scanDelay;
    
    private int _resourceCount;
    private int _amountResourcesForUnitCreate = 3;
    private int _amountResourcesForBaseCreate = 5;

    private bool _isCreateUnit = true;
    private bool _isNewBaseCreating = false;

    public event Action<Base> RequestedCreationBase;
    public event Action<Unit> SendUnitToCreateBase;

    public Flag Flag  => _flag;
    public bool EnoughUnitsToCreateBase => _units.Count >= _minCountUnitsForCreateBase;
    public int ResourceCount => _resourceCount;

    private void Awake()
    {
        _scanDelay = new WaitForSeconds(_scanDelayTime);
    }

    private void OnEnable()
    {
        StartCoroutine(Working());
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

    public void Initialize(ResourcesDatabase resourcesDatabase) => 
        _resourcesDatabase = resourcesDatabase;

    public void PrepareCreateBase()
    {
        _isCreateUnit = false;
        _isNewBaseCreating = true;
    }

    public void AddUnit(Unit unit) =>
       _units.Add(unit);

    public void SpendResourcesCreatingBase() =>
        _resourceCount -= _amountResourcesForBaseCreate;

    public void ActivateBasicBehavior()
    {
        _isCreateUnit = true;
        _isNewBaseCreating = false;
    }

    private void RemoveUnit(Unit unitToRemove) => 
        _units.Remove(unitToRemove);

    private void ReceiveResource()
    {
        _resourceCount++;
        Create();
    }

    private IEnumerator Working()
    {
        while(enabled)
        {
            yield return _scanDelay;

            SendUnit();
        }
    }

    private Unit GetFreeUnit() =>
       _units.FirstOrDefault(unit => unit.IsBusy == false);

    private void SendUnit()
    {
        Unit freeUnit = GetFreeUnit();

        if (freeUnit != null)
        {
            if(_units.Count >= _minCountUnitsForCreateBase && _resourceCount >= _amountResourcesForBaseCreate)
                SendUnitToDestination(freeUnit, () => SendUnitToCreateBase?.Invoke(freeUnit));
            else
                SendUnitsForResources(freeUnit);
        }
    }

    private void SendUnitsForResources(Unit unit) 
    {
            List<Resource> freeResources = _resourcesDatabase.GetFreeResources(_scanner.Scan()).ToList();

            if (freeResources.Count > 0)
            {
                Resource resource = freeResources[0];
                _resourcesDatabase.ReserveResources(resource);
                unit.SendToResource(resource);
            }    
    }

    private void SendUnitToDestination(Unit freeUnit, Action callback)
    {
        freeUnit.MoveToFlag(_flag, () =>
        {
            RemoveUnit(freeUnit);
            callback?.Invoke();
        });
    }

    private void Create()
    {
        if (_isCreateUnit == true)
            CreateUnit();
        else 
            RequestCreateBase();  
    }

    private void CreateUnit()
    {
        if (_isCreateUnit == true && _resourceCount >= _amountResourcesForUnitCreate)
        {
            Unit newUnit = _spawnerUnits.Spawn(transform.position);
            _units.Add(newUnit);
            _resourceCount -= _amountResourcesForUnitCreate;
        }
    }

    private void RequestCreateBase()
    {
        if (EnoughUnitsToCreateBase && _isNewBaseCreating == false && _resourceCount >= _amountResourcesForBaseCreate)
        {
            _isNewBaseCreating = true;
            RequestedCreationBase?.Invoke(this);
        }             
    }
}