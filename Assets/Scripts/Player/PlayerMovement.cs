using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private Rigidbody _rgbd;
    [SerializeField] private PlayerDataSO _playerData;

    private float _xAxi;
    private float _yAxi;
    private float _hover;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        EventManager.StartListening("OnRespawn", OnRespawn);   
    }

    private void OnDisable()
    {
        EventManager.StopListening("OnRespawn", OnRespawn);
    }

    private void OnRespawn(object[] obj)
    {
        transform.position = Utils.GetRandomSpawnPoint();
    }

    public void Move(NetworkInputData networkInput)
    {
        _yAxi = networkInput.yMovement * _playerData.ForwardSpeed;
        _xAxi = networkInput.xMovement * _playerData.SideSpeed;
        _hover = networkInput.hoverMovement * _playerData.HoverSpeed;

        if (_yAxi != 0 || _xAxi != 0 || _hover != 0)
        {
            _rgbd.MovePosition(
                transform.position + 
                transform.forward * (_yAxi * Time.fixedDeltaTime) + 
                transform.right * (_xAxi * Time.fixedDeltaTime) + 
                transform.up * (_hover * Time.fixedDeltaTime)
                );   
        }

        _rgbd.MoveRotation(networkInput.aimForwardVector);
    }
}
