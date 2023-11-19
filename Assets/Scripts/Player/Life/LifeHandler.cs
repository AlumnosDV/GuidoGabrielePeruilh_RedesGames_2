using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System;
using UnityEngine.UI;

public class LifeHandler : NetworkBehaviour
{
    private const byte FULL_LIFE = 100;
    [Networked(OnChanged = nameof(OnLifeChanged))]
    private byte CurrentLife { get; set; }
    
    [SerializeField] private RawImage _uiOnHitImage;
    [SerializeField] private Color _uiOnHitColor;
    [SerializeField] private GameObject _visualObject;
    [SerializeField] private byte _livesAmount = 3;
    
    [Networked(OnChanged = nameof(OnDeadChanged))]
    private bool IsDead { get; set; }

    public event Action OnRespawn = delegate { };
    public event Action<bool> OnEnableController = delegate {  };

    private HitboxRoot _hitBoxRoot;

    private void Awake()
    {
        _hitBoxRoot = GetComponentInChildren<HitboxRoot>();
    }

    public override void Spawned()
    {
        CurrentLife = FULL_LIFE;
    }

    public void TakeDamage(byte dmg)
    {
        Debug.Log("TakDamege");
        if (dmg > CurrentLife) dmg = CurrentLife;
        
        CurrentLife -= dmg;

        if (CurrentLife != 0) return;
        
        _livesAmount--;

        if (_livesAmount == 0)
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
        CurrentLife = FULL_LIFE;

        OnRespawn();
    }

    static void OnLifeChanged(Changed<LifeHandler> changed)
    {
        //TODO: floating life bars.
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

        //Si ahora esta muerto (IsDead == true)
        if (currentDead)
            changed.Behaviour.RemoteDead();
        else if (oldDead && !currentDead) //Si ahora no estoy muerto pero antes si
            changed.Behaviour.RemoteRespawn();

    }

    void RemoteDead()
    {
        _visualObject.SetActive(false);
        _hitBoxRoot.HitboxRootActive = false;

        OnEnableController(false);
    }

    void RemoteRespawn()
    {
        _visualObject.SetActive(true);
        _hitBoxRoot.HitboxRootActive = true;

        OnEnableController(true);
    }
    
    void DisconnectInputAuthority()
    {
        //Si este objeto que murio no tiene autoridad de input
        if (!Object.HasInputAuthority)
        {
            //Entonces desconecto al cliente que si la tiene sobre este objeto
            Runner.Disconnect(Object.InputAuthority);
        }
        else //Sino, quiero decir que el que murio es el Host
        {
            //Activar el canvas de que perdio el Host
        }

        //Despawneo este jugador ya que murio
        Runner.Despawn(Object);
    }
    

}
