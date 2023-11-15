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
    }

    private void Update()
    {
        _inputData.xMovement = Input.GetAxis("Horizontal");
        _inputData.yMovement = Input.GetAxis("Vertical");
        _inputData.hoverMovement = Input.GetAxis("Hover");

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
