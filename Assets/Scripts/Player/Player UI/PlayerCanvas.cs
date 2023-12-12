using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCanvas : MonoBehaviour
{
    [SerializeField] private GameObject _aim;
    [SerializeField] private GameObject _hitImage;
    [SerializeField] private NetworkPlayer _myNetworkPlayer;

    private void Awake()
    {
        _aim.SetActive(true);
        _hitImage.SetActive(true);
    }

    public void SetEndGameScreen(bool dead)
    {
        _aim.SetActive(dead);
    }
}
