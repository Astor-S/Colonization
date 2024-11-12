using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private float _speed = 2f;

    private Transform _targetTransform;
    private Coroutine _moveCoroutine;

    public void MoveTo(Transform target)
    {
        _targetTransform = target;

        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);
        
        _moveCoroutine = StartCoroutine(MoveTowardsTarget());
    }

    private IEnumerator MoveTowardsTarget()
    {
        while (_targetTransform != null)
        {
            transform.position = Vector3.MoveTowards(
                 transform.position, _targetTransform.position, _speed * Time.deltaTime);

            yield return null;
        }
    }
}