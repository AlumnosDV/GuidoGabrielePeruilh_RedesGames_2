using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalCamaraHandler : MonoBehaviour
{
    private Camera _localCamera;
    [SerializeField] private Transform _cameraAnchorPoint;
    [SerializeField] private PlayerDataSO _playerData;
    private Vector2 _viewInput;
    private Vector2 _screenCenter;
    private Vector2 _mouseDistance;
    private float _rollMovement;

    public float RotationX { get; private set; }
    public float RotationY { get; private set; }
    public float RotationZ { get; private set; }

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

        _mouseDistance.x = (_viewInput.x - _screenCenter.x) / _screenCenter.x;
        _mouseDistance.y = (_viewInput.y - _screenCenter.y) / _screenCenter.y;
        _mouseDistance = Vector2.ClampMagnitude(_mouseDistance, 1f);

        RotationX = -_mouseDistance.y * _playerData.LookRotateSpeed * Time.fixedDeltaTime;
        RotationY = _mouseDistance.x * _playerData.LookRotateSpeed * Time.fixedDeltaTime;
        RotationZ = _rollMovement * _playerData.RollSpeed * Time.fixedDeltaTime;

        _localCamera.transform.Rotate(RotationX, RotationY, RotationZ, Space.Self);
    }

    public void SetViewInputVector(Vector2 viewInput, Vector2 screenCenter, float rollMovement)
    {
        _viewInput = viewInput;
        _screenCenter = screenCenter;
        _rollMovement = rollMovement;
    }
}
