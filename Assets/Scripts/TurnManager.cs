using Harmonies.Environment;
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

    private StateMachine _stateMachine;

    //starts with 0
    private int _indexActualPlayer = -1;
    public int IndexActualPlayer => _indexActualPlayer;
    public int MaxPlayersCount { get; private set; }
    [SerializeField]
    private PlayerInfo[] _playerInfo;

    /// <summary>
    /// int - previous player, int - actual player.
    /// </summary>
    public Action<int, int> OnRoundEnded;
    public Action<int> OnGameStarted;
    public PlayerInfo GetActualPlayerInfo => _playerInfo[_indexActualPlayer];
    public IEnumerable<BoardSceneGenerator> GetAllBoardSceneGenerators => _playerInfo.Select(n => n.Board);

    [Inject]
    public void Construct(StateMachine stateMachine, SpawnBlocksController spawnBlocksController, EnvironmentController environmentController)
    {
        _stateMachine = stateMachine;
        stateMachine.TurnManager = this;
        MaxPlayersCount = 2;

        _spawnBlocksController = spawnBlocksController;
        _environmentController = environmentController;
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
        for (int i = 0; i < _playerInfo.Length; i++)
            _playerInfo[i].Board.Init(i);

        _stateMachine.BlocksSelectState();

        StartGameForAllClientRpc();
    }

    [ClientRpc]
    public void StartGameForAllClientRpc()
    {
        _indexActualPlayer = 0;
        OnGameStarted?.Invoke(_indexActualPlayer);
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
        int previousPlayerIndex = _indexActualPlayer;
        _indexActualPlayer++;
        if (IndexActualPlayer >= MaxPlayersCount)
            _indexActualPlayer = 0;

        RoundEndedFroAllClientRpc(previousPlayerIndex, IndexActualPlayer);
    }

    [ClientRpc]
    private void RoundEndedFroAllClientRpc(int prev, int next)
    {
        _indexActualPlayer = next;
        OnRoundEnded?.Invoke(prev, next);


        //need?
        if (_indexActualPlayer == (int)NetworkManager.Singleton.LocalClientId)
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