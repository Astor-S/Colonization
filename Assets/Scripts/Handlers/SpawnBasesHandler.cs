using System.Collections.Generic;
using UnityEngine;

public class SpawnBasesHandler : MonoBehaviour
{
    [SerializeField] private SpawnerBases _spawnerBases;
    [SerializeField] private List<Base>  _bases;

    private void OnEnable()
    {         
        foreach (Base @base in _bases)
        {
            @base.RequestedCreationBase += CreateBase;
        }
    }

    private void OnDisable()
    {
        foreach (Base @base in _bases)
        {
            @base.RequestedCreationBase -= CreateBase;
        }
    }

    private void CreateBase(Base baseToCreate)
    {
        if (baseToCreate != null && baseToCreate.Flag != null)
        {
            Unit freeUnit = baseToCreate.GetFreeUnit(); 

            if (freeUnit != null)
            {
                freeUnit.SendToFlag(baseToCreate.Flag);
                Base newBase = _spawnerBases.Spawn(baseToCreate.Flag.transform.position);
                _bases.Add(newBase);
                newBase.RequestedCreationBase += CreateBase;
                freeUnit.ChangeOwner(newBase);
                baseToCreate.RemoveUnit(freeUnit);
                baseToCreate.SpendResourcesCreatingBase();
                baseToCreate.Flag.Destroy();
                baseToCreate.ActivateBasicBehavior();
            }
        }
    }
}