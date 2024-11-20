using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    private const int SelectButton = 0;

    private Base _selectedBase;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(SelectButton))
            HandleMouseClick();
    }

    private void HandleMouseClick()
    {
        if (Physics.Raycast(_mainCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            if (hit.collider.TryGetComponent(out Base baseObject))
                _selectedBase = baseObject;
            else if (_selectedBase != null && hit.collider.TryGetComponent(out Ground _))
                PlaceFlag(hit.point);
        }
    }

    private void PlaceFlag(Vector3 position)
    {
        if (_selectedBase != null && _selectedBase.Flag.gameObject.activeInHierarchy == false)
        {
            _selectedBase.PrepareCreateBase();
            _selectedBase.Flag.transform.position = position;
            _selectedBase.Flag.gameObject.SetActive(true);
        }
        else if (_selectedBase != null && _selectedBase.Flag.gameObject.activeInHierarchy)
        {
            _selectedBase.Flag.transform.position = position;
        }

        _selectedBase = null;
    }
}