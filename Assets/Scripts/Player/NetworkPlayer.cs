using UnityEngine;
using Fusion;
using System;
using System.Collections;

public class NetworkPlayer : NetworkBehaviour
{
    public static NetworkPlayer Local { get; private set; }

    public event Action OnPlayerDespawned = delegate { };
    public event Action OnStartGame = delegate { };
    public event Action<bool> OnPlayerLose = delegate { };

    private NicknameItem _myNickname;
    public LocalCamaraHandler localCameraHandler;
    private NetworkCanvasHandler _networkCanvasHandler;
    [SerializeField] private Transform playerModel;
    [SerializeField] private GameObject _localCanvas;

    [Networked(OnChanged = nameof(OnNicknameChanged))]
    private string Nickname { get; set; }

    private void Awake()
    {
        _networkCanvasHandler = GetComponent<NetworkCanvasHandler>();
    }
    public override void Spawned()
    {
        base.Spawned();
        gameObject.name = $"Nombre_{UnityEngine.Random.Range(0, 20)}";
        _myNickname = NicknamesHandler.Instance.CreateNewNickname(this);

        if (Object.HasInputAuthority)
        {
            Local = this;
    
            var savedNickname = PlayerPrefs.GetString("PlayerNickName");

            RPC_SetNewNickname(savedNickname);
            
            Camera.main.gameObject.SetActive(false);
        }
        else
        {
            Camera localCamara = GetComponentInChildren<Camera>();
            localCamara.enabled = false;

            AudioListener audioListener = GetComponentInChildren<AudioListener>();
            audioListener.enabled = false;

            _localCanvas.SetActive(false);
        }

        Runner.SetPlayerObject(Object.InputAuthority, Object);
        

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

    public void PlayerLeft(bool isHost, bool isDead)
    {
        if (isHost)
            _networkCanvasHandler.SetHostLeftCanvas();
        else
            OnPlayerLose(isDead);

        OnPlayerDespawned();
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        OnPlayerDespawned();
    }

}
