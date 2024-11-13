using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Base : MonoBehaviour, IDestroyable<Base>
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
    public event Action<Base> Destroyed;

    public Flag Flag  => _flag;
    public bool EnoughUnitsToCreateBase => _units.Count >= _minCountUnitsForCreateBase;

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

    public void Initialize(ResourcesDatabase resourcesDatabase)
    {
        _resourcesDatabase = resourcesDatabase;
    }

    public void PlaceFlag(Flag flag)
    {
        if (EnoughUnitsToCreateBase)
        {
            _flag = flag;
            flag.transform.SetParent(transform);

            _isCreateUnit = false;
        }
    }

    public void PrepareCreateBase()
    {
            _isCreateUnit = false; 
    }

    public void MoveFlag(Flag newFlag)
    {
        if (Flag != null)
            Flag.Destroy();

        _flag = newFlag;
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

    public void AddUnit(Unit unit)
    {
        _units.Add(unit);
    }

    public void SpendResourcesCreatingBase()
    {
        _resourceCount -= _amountResourcesForBaseCreate;
    }

    public void ActivateBasicBehavior()
    {
        _isCreateUnit = true;
        _isNewBaseCreating = false;
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