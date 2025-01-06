using Harmonies.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
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
    private TextMeshProUGUI textId, textActualId, _textIpAdress;

    [SerializeField]
    private TextMeshProUGUI _scoreMainPlayer;
    [SerializeField]
    private Image _iconMainPlayer;

    [SerializeField]
    private TextMeshProUGUI[] _scoreOtherPlayer;
    [SerializeField]
    private Image[] _iconOtherPlayer;

    [SerializeField]
    private Color[] _playerColors;
    [SerializeField]
    private Sprite[] _spritesColors;

    private Dictionary<ulong, PlayerInfoElement> _playersElements = new Dictionary<ulong, PlayerInfoElement>();
    public List<PlayerInfoElement> GetListPlayerInfoElement => _playersElements.Values.ToList();

    [Inject]
    public void Construct(TurnManager turnManager, NetworkPlayersController networkPlayersController)
    {
        turnManager.OnGameStarted += OnGameStarted;
        turnManager.OnRoundEnded += OnRoundChanged;
        networkPlayersController.OnIdPlayersCreate += CreatePlayersElements;
    }

    private void Awake()
    {
        textId.text = string.Empty;
        textActualId.text = string.Empty;
        (UserType userType, string ip, ushort port) = DataConnecterController.Singlton.GetData;

        _scoreMainPlayer.text = string.Empty;
        for (int i = 0; i < _scoreOtherPlayer.Length; i++)
        {
            _scoreOtherPlayer[i].text = string.Empty;
            SetActivePlayer(i, false);
        }

        if (userType == UserType.None)
        {
            _hostButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartHost();
            });

            _clientButton.onClick.AddListener(() =>
            {
                NetworkManager.Singleton.StartClient();
            });
        } else
        {
            _textIpAdress.text += GetLocalIPAddress();
            _hostButton.gameObject.SetActive(false);
            _clientButton.gameObject.SetActive(false);    

            if(userType == UserType.Host)
            {
                StartCoroutine(ConnectTimer(() =>
                {
                    NetworkManager.Singleton.StartHost();
                    Debug.Log($"Host started");
                }));
                
            } else if (userType == UserType.Client)
            {
                StartCoroutine(ConnectTimer(() =>
                {
                    SetConnectionData(ip, port);
                    NetworkManager.Singleton.StartClient();
                    Debug.Log($"Client connecting to {ip}:{port}");
                }));
            }
        }
    }

    private IEnumerator ConnectTimer(Action action)
    {
        yield return new WaitForSeconds(0.2f);
        action();
    }

    private static string GetLocalIPAddress()
    {
        try
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork) // IPv4
                {
                    return ip.ToString();
                }
            }
            return "No IPv4 address found";
        }
        catch (SocketException ex)
        {
            Debug.LogError($"Error retrieving local IP: {ex.Message}");
            return "Error retrieving IP";
        }
    }
    private void SetConnectionData(string ipAddress, ushort port)
    {
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        if (transport != null)
        {
            transport.SetConnectionData(ipAddress, port);
        }
        else
        {
            Debug.LogError("UnityTransport is not attached to the NetworkManager!");
        }
    }

    public void CreateNewPrefabPlayer(ulong id, int count)
    {
        count--;
        for (int i = 0; i < _scoreOtherPlayer.Length; i++)
        {
            if(i < count)
                SetActivePlayer(i, true);
            else
                SetActivePlayer(i, false);
        }

        /* if (_panelPlayers.childCount != count)
             for (int i = 0; i < count - wasChildren; i++)
                 Instantiate(_prefabPlayer, _panelPlayers); */
    }

  
    public void SetActivePlayer(int index, bool enabled)
    {
        _scoreOtherPlayer[index].gameObject.SetActive(enabled);
        _iconOtherPlayer[index].gameObject.SetActive(enabled);
    }
    public override void OnNetworkSpawn() => textId.text = $"Player ID: {NetworkManager.Singleton.LocalClientId}";

    private void OnGameStarted(ulong next) => textActualId.text = $"Actual Player ID: " + next;
    private void OnRoundChanged(ulong previous, ulong next) => textActualId.text = $"Actual Player ID: " + next;

    public void CreatePlayersElements(List<ulong> ids)
    {
        int actual = 0;
        for(int i = 0; i < ids.Count; i++)
        {
            if (actual >= _scoreOtherPlayer.Length)
                actual = 0;

            TextMeshProUGUI text = _scoreOtherPlayer[actual];
            Image image = _iconOtherPlayer[actual];
            Color color = _playerColors[i];
            image.sprite = _spritesColors[i];

            if (NetworkManager.Singleton.LocalClientId == ids[i])
            {
                text = _scoreMainPlayer;
                image = _iconMainPlayer;
            }
            else
                actual++;

            _playersElements.Add(ids[i], new PlayerInfoElement(text, "Player " + ids[i], image,
                color, NetworkManager.Singleton.LocalClientId == ids[i]));
            _playersElements[ids[i]].UpdateInfo(ids[i], 0);
        }
    }

    public void MakePlayerSelected(ulong id, bool enable)
    {
        if(enable)
        {
            _playersElements[id].TextScore.rectTransform.localScale *= 1.5f;
            _playersElements[id].Image.rectTransform.localScale *= 1.5f;
        }
        else {
            _playersElements[id].TextScore.rectTransform.localScale = Vector3.one;
            _playersElements[id].Image.rectTransform.localScale = Vector3.one;
        }
    }

    public void UpdatePlayerInfo(ulong id, int newScore) => _playersElements[id].UpdateInfo(id, newScore);
}
