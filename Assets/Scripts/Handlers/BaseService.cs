using UnityEngine;

public class BaseService : MonoBehaviour
{
    [SerializeField] private Base _startBase;
    [SerializeField] private SpawnerBases _spawnerBases;
    [SerializeField] private SpawnerUnits _spawnerUnits;

    private ResourcesDatabase _resourcesDatabase = new();

    private void Start()
    { 
        _startBase.Initialize(_resourcesDatabase, _spawnerUnits);
    }

    private void OnEnable()
    {
        _startBase.SendUnitToCreateBase += OnSendUnitToCreateBase;
    }

    private void OnDisable()
    {
        _startBase.SendUnitToCreateBase -= OnSendUnitToCreateBase;
    }

    private void OnSendUnitToCreateBase(Unit unit)
    {
        unit.Reached += OnReached;
    }

    private void OnReached(Unit unit)
    {
        unit.Reached -= OnReached;
        Base @base = _spawnerBases.Spawn(unit.transform.position);
        @base.SendUnitToCreateBase += OnSendUnitToCreateBase;
        @base.Initialize(_resourcesDatabase, _spawnerUnits);
        @base.AddUnit(unit);
        unit.ChangeOwner(@base);
    }
}