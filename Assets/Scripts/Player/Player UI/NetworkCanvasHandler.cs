using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkCanvasHandler : NetworkBehaviour
{
    private PlayerCanvas _playerCanvas;

    private void Awake()
    {
        Debug.Log("NetworkCanvas Start");
        GetComponent<NetworkPlayer>().OnPlayerLose += HandlePlayerKilled;
    }

    public void SetHostLeftCanvas()
    {
        RPC_SetHostLeft();
    }

    public void HandlePlayerKilled(bool playerDead)
    {
        RPC_PlayerLost(playerDead);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.InputAuthority)]
    private void RPC_PlayerLost(bool isDead)
    {
        if (_playerCanvas == null)
            _playerCanvas = NetworkPlayer.Local.localCameraHandler.GetComponentInChildren<PlayerCanvas>();

        if (_playerCanvas != null)
            _playerCanvas.SetEndGameScreen(isDead);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_SetHostLeft()
    {
        if (_playerCanvas == null)
            _playerCanvas = NetworkPlayer.Local.localCameraHandler.GetComponentInChildren<PlayerCanvas>();

        if (_playerCanvas != null)
        {
            //_playerCanvas.SetHostLeftScreen();
            StartCoroutine(GoToMainMenuCO());
        }
    }


    IEnumerator GoToMainMenuCO()
    {
        yield return new WaitForSeconds(4f);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
