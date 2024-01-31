using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{

    [SerializeField] private Spawner _spawner;
    [SerializeField] private GameObject _selectTeamCanvas;

    private void Start()
    {
        _selectTeamCanvas.SetActive(true);
        _spawner = FindObjectOfType<Spawner>();
    }

    public void SetTeam(NetworkPlayer prefab)
    {
        _spawner.PlayerSpawner(prefab);
        _selectTeamCanvas.SetActive(false);
    }
    


}
