using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class NetworkPlayersController : NetworkBehaviour
{
    private NetworkManagerUI _networkManagerUI;
    private TurnManager _turnManager;
    private List<ulong> _idPlayers = new List<ulong>();

    [Inject]
    public void Construct(NetworkManagerUI networkManagerUI, TurnManager turnManager)
    {
        _networkManagerUI = networkManagerUI;
        _turnManager = turnManager;
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnClientConnected(ulong clientId)
    {
        if(IsOwner)
            OnClientConnectClientRpc(clientId, NetworkManager.Singleton.ConnectedClients.Count);
    }

    [ClientRpc]
    private void OnClientConnectClientRpc(ulong clientId, int countClients) => _networkManagerUI.CreateNewPrefabPlayer(clientId, countClients);

    [ServerRpc]
    public void SyncAllPlayersInfoServerRpc()
    {
        foreach (ulong id in NetworkManager.Singleton.ConnectedClients.Keys)
            SyncAllPlayersClientRpc(id);

        CreateDictElementsClientRpc();
    }

    [ClientRpc]
    private void SyncAllPlayersClientRpc(ulong clientId) => _idPlayers.Add(clientId);

    [ClientRpc]
    private void CreateDictElementsClientRpc()
    {
        Debug.Log(_idPlayers.Count);
        _networkManagerUI.CreatePlayersElements(_idPlayers);
        _turnManager.PlayersId = _idPlayers;
    }

    public void ShowPlayerElement(ulong id, bool enabled) => _networkManagerUI.MakePlayerSelected(id, enabled);
}
