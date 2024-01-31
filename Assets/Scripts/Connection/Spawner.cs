using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using System.Linq;

public class Spawner : MonoBehaviour, INetworkRunnerCallbacks
{
    private LocalPlayerInputs _playerInputs;
    private NetworkRunner _currentNetworkRunner;
    private PlayerRef _currentPlayerRef;

    public void PlayerSpawner(NetworkPlayer playerPrefab)
    {
        if (_currentNetworkRunner.IsServer)
        {
            _currentNetworkRunner.Spawn(playerPrefab, Utils.GetRandomSpawnPoint(), Quaternion.identity, _currentPlayerRef);
        }
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log($"On Player Join {runner.name}, {player.PlayerId}, {runner.IsServer}");
        if (runner.IsServer)
        {
            _currentNetworkRunner = runner;
            _currentPlayerRef = player;
            //runner.Spawn(_playerPrefab, Utils.GetRandomSpawnPoint(), Quaternion.identity, player);
        }

    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (!NetworkPlayer.Local) return;

        if (!_playerInputs)
            _playerInputs = NetworkPlayer.Local.GetComponent<LocalPlayerInputs>();
        else
            input.Set(_playerInputs.GetLocalInputs());

    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        runner.Shutdown();
    }
    
    #region Unused Callbacks
    
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    
    public void OnConnectedToServer(NetworkRunner runner) 
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnSceneLoadStart(NetworkRunner runner) { }
    
    #endregion
}
