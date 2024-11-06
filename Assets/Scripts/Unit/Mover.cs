using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;

    private Transform _targetTransform;

    private void Update()
    {
        if (_targetTransform != null)
        {
            transform.position = Vector3.MoveTowards
                (transform.position, _targetTransform.position, _speed * Time.deltaTime);
        }
    }

    public void MoveTo(Transform target)
    {
        _targetTransform = target;
    }
}