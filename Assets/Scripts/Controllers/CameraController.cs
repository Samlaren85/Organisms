using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] public bool _following;
    [SerializeField] private OrganismController _organismControl;

    private Vector3 _origin;
    private Vector3 _difference;
    private Vector2 _input;
    [SerializeField] private bool _isDragging;
    [SerializeField] private bool _isMoving;
    private Bounds _cameraBounds;
    private Vector3 _targetPosition;

    private void Start()
    {
        var height = _camera.orthographicSize;
        var width = height * _camera.aspect;

        var minX = GameSettings.WORLDBOUNDS.min.x + width;
        var maxX = GameSettings.WORLDBOUNDS.extents.x - width;

        var minY = GameSettings.WORLDBOUNDS.min.y + height;
        var maxY = GameSettings.WORLDBOUNDS.extents.y - height;

        _cameraBounds = new Bounds();
        _cameraBounds.SetMinMax(
            new Vector3(minX, minY, 0.0f),
            new Vector3(maxX, maxY, 0.0f)
            );
    }

    void Awake()
    {
        _camera = Camera.main;
        _following = false;
        _organismControl = FindObjectOfType<OrganismController>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (_following && !_organismControl.gameObject.IsUnityNull())
        {
            Vector3 newPosition = _organismControl._selectedGameObject.transform.position;
            newPosition.z = _camera.transform.position.z;
            _camera.transform.position = newPosition;
        }
    }

    public void MouseMoveCamera(InputAction.CallbackContext context)
    {
        if (context.started)  _origin = GetMousePosition;
        _isDragging = context.started || context.performed;
    }

   

    private void LateUpdate()
    {
        if (!_isDragging && !_isMoving) return;

        if (_isDragging )
        {
            _difference = GetMousePosition - transform.position;

            _targetPosition = _origin - _difference;
            _targetPosition = GetCameraBounds();

            transform.position = _targetPosition;
        }
        else if (_isMoving )
        {
            _targetPosition = (transform.position + new Vector3(_input.x * GameSettings.SCROLLSPEED, _input.y * GameSettings.SCROLLSPEED, 0));
            _targetPosition = GetCameraBounds();

            transform.position = _targetPosition;
        }
    }

    public void KeyboardMoveCamera(InputAction.CallbackContext context)
    {
        if (context.started) _input = context.ReadValue<Vector2>();
        _isMoving = context.started || context.performed;
    }

    private Vector3 GetCameraBounds()
    {
        return new Vector3(
            Mathf.Clamp(_targetPosition.x, _cameraBounds.min.x, _cameraBounds.max.x),
            Mathf.Clamp(_targetPosition.y, _cameraBounds.min.y, _cameraBounds.max.y),
            transform.position.z
            );

    }

    private Vector3 GetMousePosition => _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
}
