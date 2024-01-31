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
    [SerializeField] private LayerMask _obtacleLayerMask;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerGun = GetComponent<PlayerGun>();
        _localCamaraHandler = GetComponentInChildren<LocalCamaraHandler>();
    }

    private void OnEnable()
    {
        EventManager.StartListening("OnEnabledController", DesactiveController);
    }

    private void OnDisable()
    {
        EventManager.StopListening("OnEnabledController", DesactiveController);
    }

    private void DesactiveController(object[] obj)
    {
        if (obj[0] == null) return;

        this.enabled = (bool)obj[0];
    }   

    public override void FixedUpdateNetwork()
    {
        if (!GetInput(out _networkInput)) return;

        _playerMovement.Move(_networkInput);
           
        if (_networkInput.isFirePressed)
            _playerGun.Shoot();
    }

    private void OnCollisionEnter(Collision obj)
    {
        Debug.Log($"{obj.gameObject.CompareTag("Obstacles")}");
        if (obj.gameObject.CompareTag("Obstacles"))
        {
            GetComponent<LifeHandler>().Crash();
        }

    }

    public void TurnBack()
    {
    }
}
