using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;
using UnityEngine.UI;

public class LifeHandler : NetworkBehaviour
{
    private NetworkPlayer _myNetworkPlayer;
    [SerializeField] private RawImage _uiOnHitImage;
    [SerializeField] private Color _uiOnHitColor;
    [SerializeField] private GameObject _visualObject;
    [SerializeField] private PlayerDataSO _playerData;

    [Networked(OnChanged = nameof(OnLifeChanged))]
    private byte CurrentLife { get; set; }
    
    [Networked(OnChanged = nameof(OnDeadChanged))]
    private bool IsDead { get; set; }

    [Networked]
    private bool PlayerDead { get; set; }

    public event Action OnRespawn = delegate { };
    public event Action<bool> OnEnableController = delegate {  };

    private void Awake()
    {
        _myNetworkPlayer = GetComponent<NetworkPlayer>();
    }

    public override void Spawned()
    {
        CurrentLife = _playerData.MaxLife;
        PlayerDead = false;
    }

    public void TakeDamage(byte dmg)
    {
        if (dmg > CurrentLife) dmg = CurrentLife;
        
        CurrentLife -= dmg;

        if (CurrentLife > 0) return;
        
        _playerData.Lives--;

        if (_playerData.Lives == 0)
            PlayerDead = true;

        if (PlayerDead)
        {
            DisconnectInputAuthority();
            return;
        }

        
        StartCoroutine(RespawnCooldown());


    }

    IEnumerator OnHitCO()
    {
        if (Object.HasInputAuthority)
            _uiOnHitImage.color = _uiOnHitColor;

        yield return new WaitForSeconds(0.2f);

        if (Object.HasInputAuthority && !IsDead)
            _uiOnHitImage.color = new Color(0,0,0,0);

    }

    IEnumerator RespawnCooldown()
    {
        IsDead = true;
        
        yield return new WaitForSeconds(2f);

        IsDead = false;

        ApplyRespawn();
    }

    void ApplyRespawn()
    {
        CurrentLife = _playerData.MaxLife;

        OnRespawn();
    }

    static void OnLifeChanged(Changed<LifeHandler> changed)
    {
        byte currentLife = changed.Behaviour.CurrentLife;
        changed.LoadOld();
        byte oldLife = changed.Behaviour.CurrentLife;

        if (currentLife < oldLife)
            changed.Behaviour.OnLifeReduced();

    }

    private void OnLifeReduced()
    {
        StartCoroutine(OnHitCO());
    }

    static void OnDeadChanged(Changed<LifeHandler> changed)
    {
        bool currentDead = changed.Behaviour.IsDead;
        
        changed.LoadOld();
        
        bool oldDead = changed.Behaviour.IsDead;

        if (currentDead)
            changed.Behaviour.RemoteDead();
        else if (oldDead && !currentDead)
            changed.Behaviour.RemoteRespawn();

    }

    void RemoteDead()
    {
        _visualObject.SetActive(false);

        OnEnableController(false);
    }

    void RemoteRespawn()
    {
        _visualObject.SetActive(true);
        _uiOnHitImage.color = new Color(0, 0, 0, 0);
        OnEnableController(true);
    }

    void DisconnectInputAuthority()
    {

        if (!Object.HasInputAuthority)
        {
            Debug.Log($"{gameObject.name} dead {PlayerDead}");
            _myNetworkPlayer.PlayerLeft(false, PlayerDead);
            StartCoroutine(DisconnectPlayerCO());
        }
        else
        {
            if (Object.HasStateAuthority)
            {
                _myNetworkPlayer.PlayerLeft(true, PlayerDead);
            }
        }

        StartCoroutine(PlayerDesawnerCO());
    }

    IEnumerator DisconnectPlayerCO()
    {
        yield return new WaitForSeconds(1.5f);
        Runner.Disconnect(Object.InputAuthority);
    }

    IEnumerator PlayerDesawnerCO()
    {
        yield return new WaitForSeconds(1.5f);
        Runner.Despawn(Object);
    }


}
