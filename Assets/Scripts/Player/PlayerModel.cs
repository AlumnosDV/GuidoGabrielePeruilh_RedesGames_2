using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerModel : NetworkBehaviour
{
    [SerializeField] private Rigidbody _rgbd;
    
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private ParticleSystem _shootParticle;
    [SerializeField] private Transform _shootPosition;

    [SerializeField] private float _life;
    [SerializeField] private float _forwardSpeed;
    [SerializeField] private float _sideSpeed;
    [SerializeField] private float _hoverSpeed;
    [SerializeField] private float _rollSpeed;
    [SerializeField] private float _lookRotateSpeed;

    private float _camaraRotation;
    private float _xAxi;
    private float _yAxi;
    private float _hover;
    private float _roll;
    private int _currentSign, _previousSign;

    private Vector2 _viewInput;
    private Vector2 _mouseDistance;
    private NetworkInputData _networkInput;
    private Camera _localCamara;

    private void Awake()
    {
        _localCamara = GetComponentInChildren<Camera>(); 
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = false;
    }

    void Update()
    {
        _camaraRotation += _viewInput.y * Time.deltaTime;
        _camaraRotation = Mathf.Clamp(_camaraRotation, -90, 90);
        _localCamara.transform.localRotation = Quaternion.Euler(_camaraRotation, 0, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    public override void FixedUpdateNetwork()
    {
        if (!GetInput(out _networkInput)) return;

        Move();
        Rotate();
    }

    public void Move()
    {
        _yAxi = _networkInput.yMovement * _forwardSpeed;
        _xAxi = _networkInput.xMovement * _sideSpeed;
        _hover = _networkInput.hoverMovement * _hoverSpeed;
        _roll = _networkInput.rollMovement * _rollSpeed;



        if (_yAxi != 0 || _xAxi != 0 || _hover != 0)
        {
            _rgbd.MovePosition(transform.position + transform.forward * (_yAxi * Time.fixedDeltaTime) + transform.right * (_xAxi * Time.fixedDeltaTime) + transform.up * (_hover * Time.fixedDeltaTime));

            _currentSign = (int)Mathf.Sign(_yAxi);

            if (_currentSign != _previousSign)
            {
                _previousSign = _currentSign;
            }
            
        }
        else if (_currentSign != 0)
        {
            _currentSign = 0;
        }
    }

    public void Rotate()
    {

        _mouseDistance.x = (_networkInput._lookInput.x - _networkInput._screenCenter.x) / _networkInput._screenCenter.x;
        _mouseDistance.y = (_networkInput._lookInput.y - _networkInput._screenCenter.y) / _networkInput._screenCenter.y;
        _mouseDistance = Vector2.ClampMagnitude(_mouseDistance, 1f);
        
        transform.Rotate(-_mouseDistance.y * _lookRotateSpeed * Time.fixedDeltaTime, _mouseDistance.x * _lookRotateSpeed * Time.fixedDeltaTime, _roll * _rollSpeed * Time.fixedDeltaTime, Space.Self);
    }

    void Shoot()
    {
        Instantiate(_bulletPrefab, _shootPosition.position, transform.rotation);
        _shootParticle.Play();
    }

    public void TakeDamage(float dmg)
    {
        
    }
}
