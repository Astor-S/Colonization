using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    private const int LeftMouseButton = 0;

    private Base _selectedBase;

    private void Update()
    {
        if (Input.GetMouseButtonDown(LeftMouseButton))
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