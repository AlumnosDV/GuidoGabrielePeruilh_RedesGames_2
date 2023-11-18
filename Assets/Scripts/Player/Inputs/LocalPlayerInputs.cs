using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerInputs : MonoBehaviour
{
    private NetworkInputData _inputData;
    private LocalCamaraHandler _localCameraHandler;

    private bool _isFirePressed;
    
    private void Awake()
    {
        _inputData = new NetworkInputData();
        _localCameraHandler = GetComponentInChildren<LocalCamaraHandler>();
        _inputData.screenCenter.x = Screen.width * 0.5f;
        _inputData.screenCenter.y = Screen.height * 0.5f;
    }

    private void Update()
    {
        _inputData.xMovement = Input.GetAxis("Horizontal");
        _inputData.yMovement = Input.GetAxis("Vertical");
        _inputData.hoverMovement = Input.GetAxis("Hover");
        _inputData.rollMovement = Input.GetAxis("Roll");


        _inputData.lookInput.x = Input.mousePosition.x;
        _inputData.lookInput.y = Input.mousePosition.y ;

        _inputData.aimForwardVector = _localCameraHandler.transform.forward;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _isFirePressed = true;
        }

        _localCameraHandler.SetViewInputVector(_inputData.lookInput, _inputData.screenCenter, _inputData.rollMovement);
    }

    public NetworkInputData GetLocalInputs()
    {
        
        _inputData.isFirePressed = _isFirePressed;
        _isFirePressed = false;
        return _inputData;
    }
}
