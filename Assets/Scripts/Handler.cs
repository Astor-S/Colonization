using UnityEngine;
using UnityEngine.EventSystems;

public class Handler : MonoBehaviour
{
    [SerializeField] private SpawnerBases _spawnerBases;
    [SerializeField] private SpawnerFlags _spawnerFlags;

    private Base _selectedBase;
    private Vector3 _flagPlacementPosition;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleMouseClick();
    }

    private void AddListeners()
    {

    }

    private void RemoveListeners()
    {

    }

    private void SpawnBase()
    {

    }

    private void HandleMouseClick()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out Base baseObject))
                SelectBase(baseObject);
            else if (_selectedBase != null && hit.collider.TryGetComponent(out Ground _))
                PlaceFlag(hit.point);       
        }
    }

    private void SelectBase(Base baseObject)
    {
        _selectedBase = baseObject;
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