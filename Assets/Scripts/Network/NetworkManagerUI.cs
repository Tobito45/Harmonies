using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class NetworkManagerUI : NetworkBehaviour
{
    [SerializeField] 
    private Button _hostButton;
    [SerializeField] 
    private Button _clientButton;
    [SerializeField]
    private TextMeshProUGUI textId, textActualId;

    [SerializeField]
    private Transform _panelPlayers;
    [SerializeField]
    private GameObject _prefabPlayer;

    private Dictionary<ulong, PlayerInfoElement> _playersElements = new Dictionary<ulong, PlayerInfoElement>();

    [Inject]
    private void Construct(NetworkPlayersController networkPlayersController) => networkPlayersController.OnIdPlayersCreate += CreatePlayersElements;

    private void Awake()
    {
        textId.text = string.Empty;
        textActualId.text = string.Empty;
        _hostButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });

        _clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }

    public void CreateNewPrefabPlayer(ulong id, int count)
    {
        int wasChildren = _panelPlayers.childCount;

        if (_panelPlayers.childCount != count)
            for (int i = 0; i < count - wasChildren; i++)
                Instantiate(_prefabPlayer, _panelPlayers);
    }

    [Inject]
    public void Construct(TurnManager turnManager)
    {
        turnManager.OnGameStarted += OnGameStarted;
        turnManager.OnRoundEnded += OnRoundChanged;
    }

    public override void OnNetworkSpawn() => textId.text = $"Player ID: {NetworkManager.Singleton.LocalClientId}";

    private void OnGameStarted(ulong next) => textActualId.text = $"Actual Player ID: " + next;
    private void OnRoundChanged(ulong previous, ulong next) => textActualId.text = $"Actual Player ID: " + next;

    public void CreatePlayersElements(List<ulong> ids)
    {
        for(int i  = 0; i < ids.Count; i++)
        {
            _playersElements.Add(ids[i], _panelPlayers.GetChild(i).gameObject.GetComponent<PlayerInfoElement>());
            _playersElements[ids[i]].UpdateInfo(ids[i], 0);
        }

        _playersElements[NetworkManager.Singleton.LocalClientId].Image.color = Color.green;
    }

    public void MakePlayerSelected(ulong id, bool enable)
    {
        if(enable) 
            _playersElements[id].Image.color = Color.yellow;
        else {
            _playersElements[id].Image.color = Color.white;

            if(id == NetworkManager.Singleton.LocalClientId)
                _playersElements[id].Image.color = Color.green;
        }
    }

    public void UpdatePlayerInfo(ulong id, int newScore) => _playersElements[id].UpdateInfo(id, newScore);
}
