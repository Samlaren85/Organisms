using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class OrganismController : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] private TextMeshProUGUI _organismInfo;
    [SerializeField] private GameObject _selectedGameObject;

    public void Awake()
    {
        _camera = Camera.main;
        _organismInfo = FindObjectsOfType<TextMeshProUGUI>().FirstOrDefault(t => t.name.Equals("OrganismInfo"));
    }

    public void SelectOrganism(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        
        RaycastHit2D hit = Physics2D.GetRayIntersection(_camera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        Debug.Log($"LeftClick {_camera.ScreenToWorldPoint(Input.mousePosition)}");

        if (!hit.collider)
        {
            if(_selectedGameObject != null)
            {
                _selectedGameObject = null;
                _organismInfo.text = string.Empty;
            }
        }
        else if (hit.collider.gameObject.CompareTag("Organism"))
        {
            _selectedGameObject = hit.collider.gameObject;
            _organismInfo.text = _selectedGameObject.name;
        }
    }

    public void MoveOrganism(InputAction.CallbackContext context)
    {
        if (!context.started || _selectedGameObject.IsUnityNull()) return;

        RaycastHit2D hit = Physics2D.GetRayIntersection(_camera.ScreenPointToRay(Mouse.current.position.ReadValue()));
        Debug.Log($"RightClick {_camera.ScreenToWorldPoint(Input.mousePosition)}");

        _selectedGameObject.transform.position = (Vector2)_camera.ScreenToWorldPoint(Input.mousePosition);
    }
}
