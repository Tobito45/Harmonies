using Harmonies.Enviroment;
using Harmonies.Selectors;
using Harmonies.States;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class TurnManager : NetworkBehaviour //to singlton?
{
    private SpawnBlocksController _spawnBlocksController;
    private EnvironmentController _environmentController;
    private NetworkPlayersController _networkPlayersController;

    private StateMachine _stateMachine;

    //starts with 0
    private int _indexActualPlayer = -1;
    public ulong IndexActualPlayer => _playersId[_indexActualPlayer];
    public int GetActualPlayerNumber => _indexActualPlayer;
    //public int MaxPlayersCount { get; private set; }
    private List<ulong> _playersId = new List<ulong>();
    public int PlayersCount => _playersId.Count;

    [SerializeField]
    private PlayerInfo[] _playerInfo;

    /// <summary>
    /// ulong - previous player, ulong - actual player
    /// </summary>
    public Action<ulong, ulong> OnRoundEnded;
    public Action<ulong> OnGameStarted;
    public PlayerInfo GetActualPlayerInfo => _playerInfo[_indexActualPlayer];
    public IEnumerable<BoardSceneGenerator> GetAllBoardSceneGenerators => _playerInfo.Select(n => n.Board);

    [Inject]
    public void Construct(StateMachine stateMachine, 
        SpawnBlocksController spawnBlocksController, 
        EnvironmentController environmentController,
        NetworkPlayersController networkPlayersController)
    {
        _stateMachine = stateMachine;
        stateMachine.TurnManager = this;
        //MaxPlayersCount = 2;

        _spawnBlocksController = spawnBlocksController;
        _environmentController = environmentController;
        _networkPlayersController = networkPlayersController;
        _networkPlayersController.OnIdPlayersCreate += (ids) => _playersId = ids;
    }

    private void Update()
    {
        if (_stateMachine.ActualState is AnimalsEnvironmentSelectState)
            if (Input.GetKeyUp(KeyCode.E))
                WasSelectedOrSkipedAnimalsEnviroment();

        if (_stateMachine.ActualState is AnimalsSelectState)
            if (Input.GetKeyUp(KeyCode.R))
                WasAnimalsSkiped();

        if (Input.GetKeyDown(KeyCode.O) && IsOwner)
            StartCoroutine(StartGame());
    }

    public IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1);
        _networkPlayersController.SyncAllPlayersInfoServerRpc();
        for (int i = 0; i < _playerInfo.Length; i++)
            _playerInfo[i].Board.Init(i);

        _stateMachine.BlocksSelectState();

        StartGameForAllClientRpc();
    }

    [ClientRpc]
    public void StartGameForAllClientRpc()
    {
        _indexActualPlayer = 0;
        _networkPlayersController.ShowPlayerElement(IndexActualPlayer, true);
        OnGameStarted?.Invoke(IndexActualPlayer);
    }

    public void SubsribeOnStateMachine(Action<IState> action) => _stateMachine.OnStateChange += action;

    public void WasSpawnedBlock()
    {
        if (_spawnBlocksController.IsAnyBlockNotPlaced()) return;

        _stateMachine.AnimalsEnvironmentSelectState();
    }

    public void WasSelectedOrSkipedAnimalsEnviroment() =>
        _stateMachine.AnimalsSelectState();

    public void WasAnimalsSkiped()
    {
        SelectNextPlayerServerRpc();

        //_stateMachine.BlocksSelectState();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SelectNextPlayerServerRpc()
    {
        ulong previousPlayerIndex = _playersId[_indexActualPlayer];
        _indexActualPlayer++;
        if (_indexActualPlayer >= _playersId.Count)
            _indexActualPlayer = 0;

        RoundEndedFroAllClientRpc(previousPlayerIndex, IndexActualPlayer, _indexActualPlayer);
    }

    [ClientRpc]
    private void RoundEndedFroAllClientRpc(ulong prev, ulong next, int index)
    {
        _indexActualPlayer = index;
        OnRoundEnded?.Invoke(prev, next);

        _networkPlayersController.ShowPlayerElement(next, true);
        _networkPlayersController.ShowPlayerElement(prev, false);
        
        if (next == NetworkManager.Singleton.LocalClientId)
            _stateMachine.BlocksSelectState();
        else
            _stateMachine.SelectState(null);

    }
    public void SpawnEnvironmentToPlayerZone() => _environmentController.CreatePlayerSelectableEnvironment();
    public void WasSelectedBlocksSelector() => _stateMachine.BlocksPlaceState();

    public bool IsAnyEnviroment() => _environmentController.IsAnyEnviroment();

}


[System.Serializable]
public class PlayerInfo
{
    [SerializeField]
    private Transform[] _environmnetsSpawns;

    [field: SerializeField]
    public BoardSceneGenerator Board { get; private set; }
    public Transform GetEnvironmentSpawn(int index) => _environmnetsSpawns[index];
}