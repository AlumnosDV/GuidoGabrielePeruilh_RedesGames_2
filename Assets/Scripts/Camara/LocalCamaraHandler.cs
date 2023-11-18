using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalCamaraHandler : MonoBehaviour
{
    private Camera _localCamera;
    [SerializeField] private Transform _cameraAnchorPoint;
    [SerializeField] private float _rollSpeed;
    [SerializeField] private float _lookRotateSpeed;
    private Vector2 _viewInput;
    private Vector2 _screenCenter;
    private Vector2 _mouseDistance;

    private float _cameraRotationX;
    private float _cameraRotationY;
    private float _rollMovement;

    private void Awake()
    {
        _localCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        if (_localCamera.enabled)
            _localCamera.transform.parent = null;
    }

    private void LateUpdate()
    {
        if (_cameraAnchorPoint == null) return;
        if (!_localCamera.enabled) return;

        _localCamera.transform.position = _cameraAnchorPoint.position;

        _cameraRotationX += _viewInput.y * Time.deltaTime;
        _cameraRotationX = Mathf.Clamp(_cameraRotationX, -90, 90);
        _cameraRotationY += _viewInput.x * Time.deltaTime;

        _mouseDistance.x = (_viewInput.x - _screenCenter.x) / _screenCenter.x;
        _mouseDistance.y = (_viewInput.y - _screenCenter.y) / _screenCenter.y;
        _mouseDistance = Vector2.ClampMagnitude(_mouseDistance, 1f);

        _localCamera.transform.Rotate(-_mouseDistance.y * _lookRotateSpeed * Time.fixedDeltaTime, _mouseDistance.x * _lookRotateSpeed * Time.fixedDeltaTime, _rollMovement * _rollSpeed * Time.fixedDeltaTime, Space.Self);
    }

    public void SetViewInputVector(Vector2 viewInput, Vector2 screenCenter, float rollMovement)
    {
        _viewInput = viewInput;
        _screenCenter = screenCenter;
        _rollMovement = rollMovement;
    }
}
