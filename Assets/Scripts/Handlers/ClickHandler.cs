using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    [SerializeField] private SpawnerFlags _spawnerFlags;

    private Base _selectedBase;
    private Vector3 _flagPlacementPosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleMouseClick();
    }

    private void HandleMouseClick()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out Base baseObject))
                _selectedBase = baseObject;
            else if (_selectedBase != null && hit.collider.TryGetComponent(out Ground _))
                PlaceFlag(hit.point);
        }
    }

    private void PlaceFlag(Vector3 position)
    {
        if (_selectedBase != null && _selectedBase.Flag == null)
        {
            _flagPlacementPosition = position;
            _selectedBase.PlaceFlag(_spawnerFlags.Spawn(_flagPlacementPosition));
            _selectedBase = null;
        }
        else if (_selectedBase != null && _selectedBase.Flag != null)
        {
            _flagPlacementPosition = position;
            _selectedBase.MoveFlag(_spawnerFlags.Spawn(_flagPlacementPosition));
            _selectedBase = null;
        }
    }
}