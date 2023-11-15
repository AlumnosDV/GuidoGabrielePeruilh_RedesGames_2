using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    [SerializeField] private Rigidbody _rgbd;
    [SerializeField] private Animator _animator;
    
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private ParticleSystem _shootParticle;
    [SerializeField] private Transform _shootPosition;

    [SerializeField] private float _life;
    [SerializeField] private float _forwardSpeed;
    [SerializeField] private float _sideSpeed;
    [SerializeField] private float _hoverSpeed;
    [SerializeField] private float _rollSpeed;
    [SerializeField] private float _lookRotateSpeed;

    private float _xAxi;
    private float _yAxi;
    private float _hover;
    private float _roll;
    private Vector2 _lookInput, _screenCenter, _mouseDistance;
    private int _currentSign, _previousSign;
    
    void Start()
    {
        _screenCenter.x = Screen.width * 0.5f;
        _screenCenter.y = Screen.height * 0.5f;

        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        _lookInput.x = Input.mousePosition.x;
        _lookInput.y = Input.mousePosition.y;

        _mouseDistance.x = (_lookInput.x - _screenCenter.x) / _screenCenter.x;
        _mouseDistance.y = (_lookInput.y - _screenCenter.y) / _screenCenter.y;
        _mouseDistance = Vector2.ClampMagnitude(_mouseDistance, 1f);
        _yAxi = Input.GetAxis("Vertical") * _forwardSpeed;
        _xAxi = Input.GetAxis("Horizontal") * _sideSpeed;
        _hover = Input.GetAxis("Hover") * _hoverSpeed;
        _roll = Input.GetAxis("Roll") * _rollSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        transform.Rotate(-_mouseDistance.y * _lookRotateSpeed * Time.fixedDeltaTime, _mouseDistance.x * _lookRotateSpeed * Time.fixedDeltaTime, _roll * _rollSpeed * Time.fixedDeltaTime, Space.Self);

        if (_yAxi != 0 || _xAxi != 0 || _hover != 0)
        {
            _rgbd.MovePosition(transform.position + Vector3.forward * (_yAxi * Time.fixedDeltaTime) + Vector3.right * (_xAxi * Time.fixedDeltaTime) + Vector3.up * (_hover * Time.fixedDeltaTime));

            _currentSign = (int)Mathf.Sign(_yAxi);

            if (_currentSign != _previousSign)
            {
                _previousSign = _currentSign;
            }
            
            _animator.SetFloat("MovementValue", Mathf.Abs(_yAxi));
        }
        else if (_currentSign != 0)
        {
            _currentSign = 0;
            _animator.SetFloat("MovementValue", 0);
        }
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
