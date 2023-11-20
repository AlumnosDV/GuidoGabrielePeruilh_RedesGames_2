using UnityEngine;
using Fusion;
using System;

public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer Local { get; private set; }

    public event Action OnPlayerDespawned = delegate { };

    private NicknameItem _myNickname;
    [SerializeField] Transform playerModel;
    
    [Networked(OnChanged = nameof(OnNicknameChanged))]
    private string Nickname { get; set; } 
    
    public override void Spawned()
    {
        base.Spawned();
        gameObject.name = $"Nombre_{UnityEngine.Random.Range(-20, 20)}";
        _myNickname = NicknamesHandler.Instance.CreateNewNickname(this);


        Debug.Log($"{gameObject.name} Object {Object}. StateAutoraty {Object.HasStateAuthority}. Input Authoraty {Object.HasInputAuthority}. Condition {!Object || !Object.HasStateAuthority}");

        if (Object.HasInputAuthority)
        {
            Local = this;
    
            var savedNickname = PlayerPrefs.GetString("nickname");

            RPC_SetNewNickname(savedNickname);
            
            Camera.main.gameObject.SetActive(false);
        }
        else
        {
            Camera localCamara = GetComponentInChildren<Camera>();
            localCamara.enabled = false;

            AudioListener audioListener = GetComponentInChildren<AudioListener>();
            audioListener.enabled = false;
        }

        if(Object.HasInputAuthority)
            Utils.SetRenderLayerInChildren(playerModel, LayerMask.NameToLayer("LocalPlayer"));
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_SetNewNickname(string newNick)
    {
        Nickname = newNick;
    }
    
    static void OnNicknameChanged(Changed<NetworkPlayer> changed)
    {
        var behaviour = changed.Behaviour;

        behaviour._myNickname.UpdateNickname(behaviour.Nickname);
    }
    
    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnPlayerDespawned();
    }
}
