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
    private GameObject _otherPlayersPanel;

    // 0 is main, 1 is big, other are other players
    [SerializeField]
    private PlayerPanelElement[] _playerPanelElements;


    [SerializeField]
    private Color[] _playerColors;
    [SerializeField]
    private Sprite[] _spritesColors;

    private Dictionary<ulong, PlayerInfoElement> _playersElements = new Dictionary<ulong, PlayerInfoElement>();
    public List<PlayerInfoElement> GetListPlayerInfoElement => _playersElements.Values.ToList();
    public PlayerInfoElement PlayerInfoById(ulong id) => _playersElements[id];

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

        //_scoreMainPlayer.text = " : 0";
        for (int i = 0; i < _playerPanelElements.Length; i++)
        {
            if (i == 0)
                _playerPanelElements[i].Text.text = " : 0";
            else
                _playerPanelElements[i].Text.text = string.Empty;
            SetActivePlayer(i, false);
        }
        _otherPlayersPanel.SetActive(false);

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
            _textIpAdress.text += "\n" + GetLocalIPAddress();
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
        //count--;
        _otherPlayersPanel.SetActive(count > 1);

        for (int i = 0; i < _playerPanelElements.Length; i++)
        {
            if(i == 1)
            {
                count++;
                continue;
            }

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
        _playerPanelElements[index].Text.gameObject.SetActive(enabled);
        _playerPanelElements[index].Image.gameObject.SetActive(enabled);
    }
    public override void OnNetworkSpawn() => textId.text = $"Player ID: {NetworkManager.Singleton.LocalClientId}";

    private void OnGameStarted(ulong next) => textActualId.text = $"Actual Player ID: " + next;
    private void OnRoundChanged(ulong previous, ulong next) => textActualId.text = $"Actual Player ID: " + next;

    public void CreatePlayersElements(List<ulong> ids)
    {
        FreeCameraMovement freeCameraMovement = FindObjectOfType<FreeCameraMovement>();
        int actual = 2;
        for(int i = 0; i < ids.Count; i++)
        {
            //if (actual >= _scoreOtherPlayer.Length)
            //    actual = 0;

            TextMeshProUGUI text = _playerPanelElements[actual].Text;
            Image image = _playerPanelElements[actual].Image;
            if(ids.Count == 2)
            {
                SetActivePlayer(actual, false);
                SetActivePlayer(1, true);
                image = _playerPanelElements[1].Image;
                text = _playerPanelElements[1].Text;
            }

            Color color = _playerColors[i];

            if (NetworkManager.Singleton.LocalClientId == ids[i])
            {
                text = _playerPanelElements[0].Text;
                image = _playerPanelElements[0].Image;
                _playerPanelElements[0].Button.onClick.AddListener(() => freeCameraMovement.SetPositionAndCenter((ulong)i));
                if (ids.Count == 2)
                    _playerPanelElements[1].Button.onClick.AddListener(() => freeCameraMovement.SetPositionAndCenter((ulong)(i + 1) % 2));
            }
            else
                actual++;

            if (ids.Count != 2)
                _playerPanelElements[actual].Button.onClick.AddListener(() => freeCameraMovement.SetPositionAndCenter((ulong)(actual - 1)));
            image.sprite = _spritesColors[i];
            _playersElements.Add(ids[i], new PlayerInfoElement(text, "Player " + ids[i], image,
                color, NetworkManager.Singleton.LocalClientId == ids[i] || image == _playerPanelElements[1].Image));
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

[System.Serializable]
public class PlayerPanelElement
{
    [field: SerializeField]
    public TextMeshProUGUI Text { get; set; }

    [field: SerializeField]
    public Image Image { get; set; }

    [field: SerializeField]
    public Button Button { get; set; }
}

