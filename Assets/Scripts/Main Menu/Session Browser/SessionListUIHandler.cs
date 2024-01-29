using UnityEngine;
using UnityEngine.UI;
using Fusion;
using TMPro;
using System.Collections.Generic;
using System;

public class SessionListUIHandler : MonoBehaviour
{
    [SerializeField] private NetworkRunnerHandler _networkHandler;
    [SerializeField] private MenuPrincipalUI _menuPrincipalUI;
    [Header("Canvas Items")]
    [SerializeField] private TextMeshProUGUI _statusText;
    [SerializeField] private GameObject _sessionItemListPrefab;
    [SerializeField] private GameObject _createSessionButton;
    [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;

    private void Awake()
    {
        ClearList();
        _createSessionButton.SetActive(false);
    }

    private void OnEnable()
    {
        EventManager.StartListening("OnSessionListUpdate", ReceiveSessionList);
    }

    private void OnDisable()
    {
        EventManager.StopListening("OnSessionListUpdate", ReceiveSessionList);
    }

    void ReceiveSessionList(object[] obj)
    {
        if (obj[0] == null) return;

        var allSessions = (List<SessionInfo>)obj[0];

        ClearList();

        if (allSessions.Count == 0)
        {
            OnNoSessionFound();
            ActiveCreateGameOption();
            return;
        }

        foreach (var session in allSessions)
        {
            AddToList(session);
        }
    }

    public void ClearList()
    {
        foreach (Transform child in _verticalLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }
        if (_statusText == null) return;
            _statusText.gameObject.SetActive(false);
    }

    public void AddToList(SessionInfo sessionInfo)
    {
        SessionInfoListUIItem addedSessionInfoListUIItem = Instantiate(_sessionItemListPrefab, _verticalLayoutGroup.transform).GetComponent<SessionInfoListUIItem>();

        addedSessionInfoListUIItem.SetInfomartion(sessionInfo);

        addedSessionInfoListUIItem.OnJoinSession += AddedSessionInfoListUIItem_OnJoinSession;
    }

    private void AddedSessionInfoListUIItem_OnJoinSession(SessionInfo sessionInfo)
    {
        _networkHandler.JoinGame(sessionInfo);
        _menuPrincipalUI.GoToJoiningSessionScreen();
    }

    public void ActiveCreateGameOption()
    {
        _createSessionButton.SetActive(true);
    }

    public void OnNoSessionFound()
    {
        ClearList();
        _statusText.text = "No Games Founded";
        _statusText.gameObject.SetActive(true);
    }

    public void OnLookingForSessions()
    {
        ClearList();
        _statusText.text = "Looking for Game Sessions";
        _statusText.gameObject.SetActive(true);
    }
}

