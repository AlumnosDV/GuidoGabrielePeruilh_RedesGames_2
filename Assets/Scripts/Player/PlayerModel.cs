using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(Rigidbody))]
public class PlayerModel : NetworkBehaviour
{
    [SerializeField] private Rigidbody _rgbd;
    [SerializeField] private float _life;
    [SerializeField] private float _forwardSpeed;
    [SerializeField] private float _sideSpeed;
    [SerializeField] private float _hoverSpeed;

    private float _xAxi;
    private float _yAxi;
    private float _hover;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = false;
    }

    public override void Spawned()
    {
        base.Spawned();
        GetComponent<LifeHandler>().OnRespawn += () => transform.position = Utils.GetRandomSpawnPoint();
    }


    public void Move(NetworkInputData networkInput)
    {
        _yAxi = networkInput.yMovement * _forwardSpeed;
        _xAxi = networkInput.xMovement * _sideSpeed;
        _hover = networkInput.hoverMovement * _hoverSpeed;

        if (_yAxi != 0 || _xAxi != 0 || _hover != 0)
        {
            _rgbd.MovePosition(transform.position + transform.forward * (_yAxi * Time.fixedDeltaTime) + transform.right * (_xAxi * Time.fixedDeltaTime) + transform.up * (_hover * Time.fixedDeltaTime));   
        }

        transform.forward = networkInput.aimForwardVector;
        Quaternion rotation = transform.rotation;
        rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z);
        transform.rotation = rotation;
    }
}
