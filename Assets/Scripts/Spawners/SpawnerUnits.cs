using UnityEngine;

public class SpawnerUnits : Spawner<Unit>
{
    [SerializeField] private Base _base;

    protected override void OnSpawn(Unit obj)
    {
        obj.Init(_base);
    }
}