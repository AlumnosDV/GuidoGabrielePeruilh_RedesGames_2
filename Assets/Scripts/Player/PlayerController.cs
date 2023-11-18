using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

[RequireComponent(typeof(PlayerModel))]
[RequireComponent(typeof(PlayerGun))]
[RequireComponent(typeof(LifeHandler))]
public class PlayerController : NetworkBehaviour
{
    private PlayerModel _playerModel;
    private PlayerGun _playerGun;
    private NetworkInputData _networkInput;

    private void Awake()
    {
        _playerModel = GetComponent<PlayerModel>();
        _playerGun = GetComponent<PlayerGun>();
        
        GetComponent<LifeHandler>().OnEnableController += (b) => enabled = b;
    }
   

    public override void FixedUpdateNetwork()
    {
        if (!GetInput(out _networkInput)) return;

        _playerModel.Move(_networkInput);
                
        //SHOOT
        if (_networkInput.isFirePressed)
        {
            _playerGun.Shoot();
        }
    }
}
