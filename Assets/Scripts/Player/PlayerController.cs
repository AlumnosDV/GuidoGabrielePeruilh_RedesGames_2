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
    private PlayerMovement _playerMovement;
    private PlayerGun _playerGun;
    private LocalCamaraHandler _localCamaraHandler;
    private NetworkInputData _networkInput;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerGun = GetComponent<PlayerGun>();
        _localCamaraHandler = GetComponentInChildren<LocalCamaraHandler>();

        GetComponent<LifeHandler>().OnEnableController += DesactiveController;
    }

    private void DesactiveController(bool active)
    {
        this.enabled = active;
    }
   

    public override void FixedUpdateNetwork()
    {
        if (!GetInput(out _networkInput)) return;

        _playerMovement.Move(_networkInput);
           
        if (_networkInput.isFirePressed)
            _playerGun.Shoot();
    }

    public void TurnBack()
    {
    }
}
