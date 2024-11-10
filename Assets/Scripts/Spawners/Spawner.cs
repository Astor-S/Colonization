using UnityEngine;

public class Spawner<T> : MonoBehaviour where T : MonoBehaviour, IDestroyable<T>
{
    [SerializeField] private T _objectPrefab;
    [SerializeField] private int _poolCapacity;
    [SerializeField] private int _poolMaxSize;

    private Pool<T> _pool;

    protected virtual void Awake()
    {
        _pool = new Pool<T>(
            () => Instantiate(_objectPrefab));
    }

    public  T Spawn(Vector3 position)
    {
        T @object = _pool.GetObject();
        OnSpawn(@object);
        @object.transform.position = position;
        @object.Destroyed += OnObjectDestroyed;
        @object.gameObject.SetActive(true);

        return @object;
    }

    protected virtual void OnSpawn(T obj) {}

    protected virtual void OnObjectDestroyed(T obj)
    {
        obj.Destroyed -= OnObjectDestroyed;
        _pool.Release(obj);
        obj.gameObject.SetActive(false);
    }
}