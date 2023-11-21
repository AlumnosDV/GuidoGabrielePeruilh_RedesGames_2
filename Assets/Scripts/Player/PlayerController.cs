using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerGun))]
[RequireComponent(typeof(LifeHandler))]
public class PlayerController : NetworkBehaviour
{
    private PlayerMovement _playerModel;
    private PlayerGun _playerGun;
    private NetworkInputData _networkInput;

    private void Awake()
    {
        _playerModel = GetComponent<PlayerMovement>();
        _playerGun = GetComponent<PlayerGun>();

        GetComponent<LifeHandler>().OnEnableController += (b) => enabled = b;
    }
   

    public override void FixedUpdateNetwork()
    {
        if (!GetInput(out _networkInput)) return;

        _playerModel.Move(_networkInput);
                
        if (_networkInput.isFirePressed)
        {
            _playerGun.Shoot();
        }
    }

    public void TurnBack()
    {
    }
}
