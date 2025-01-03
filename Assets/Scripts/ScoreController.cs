using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class ScoreController : NetworkBehaviour
{
    public static int WaterNumber { get; set; } = 0;

    private NetworkManagerUI _networkManagerUI;

    private Dictionary<ulong, int> score = new Dictionary<ulong, int>();

    [Inject]
    private void Construct(NetworkPlayersController networkPlayersController, NetworkManagerUI networkManagerUI)
    {
        networkPlayersController.OnIdPlayersCreate += CreateDictionaryScore;
        _networkManagerUI = networkManagerUI;
    }

    public void UpdateScore(int addScore) => 
        UpdateServerRpc(NetworkManager.Singleton.LocalClientId, score[NetworkManager.Singleton.LocalClientId] + addScore);

    [ServerRpc(RequireOwnership = false)]
    private void UpdateServerRpc(ulong id, int newScore) => ChangeScoreClientRpc(id, newScore);


    [ClientRpc]
    private void ChangeScoreClientRpc(ulong id, int newScore)
    {
        score[id] = newScore;
        ShowInfoPlayer(id);
    }

    private void ShowInfoPlayer(ulong id) => _networkManagerUI.UpdatePlayerInfo(id, score[id]);

    private void CreateDictionaryScore(List<ulong> ids)
    {
        foreach (ulong id in ids)
            score.Add(id, 0);
    }

}
