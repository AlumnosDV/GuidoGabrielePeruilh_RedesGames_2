using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerInputs : MonoBehaviour
{
    private NetworkInputData _inputData;

    private bool _isFirePressed;
    
    private void Awake()
    {
        _inputData = new NetworkInputData();
        _inputData._screenCenter.x = Screen.width * 0.5f;
        _inputData._screenCenter.y = Screen.height * 0.5f;
    }

    private void Update()
    {
        _inputData.xMovement = Input.GetAxis("Horizontal");
        _inputData.yMovement = Input.GetAxis("Vertical");
        _inputData.hoverMovement = Input.GetAxis("Hover");
        _inputData.rollMovement = Input.GetAxis("Roll");
        _inputData._lookInput.x = Input.mousePosition.x;
        _inputData._lookInput.y = Input.mousePosition.y ;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _isFirePressed = true;
        }
    }

    public NetworkInputData GetLocalInputs()
    {
        
        _inputData.isFirePressed = _isFirePressed;
        _isFirePressed = false;
        return _inputData;
    }
}
