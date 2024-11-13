using Harmonies.Environment;
using Harmonies.Selectors;
using Harmonies.States;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TurnManager : MonoBehaviour
{
    private SpawnBlocksController _spawnBlocksController;
    private EnvironmentController _environmentController;

    private StateMachine _stateMachine;

    //starts with 0
    public int IndexActualPlayer { get; private set; }
    public int MaxPlayersCount  { get; private set; }
    [SerializeField]
    private PlayerInfo[] _playerInfo;
    /// <summary>
    /// int - previous player, int - actual player
    /// </summary>
    public Action<int, int> OnRoundEnded; 
    public PlayerInfo GetActualPlayerInfo() => _playerInfo[IndexActualPlayer];

    [Inject]
    public void Construct(StateMachine stateMachine, SpawnBlocksController spawnBlocksController, EnvironmentController environmentController)
    {
        _stateMachine = stateMachine;
        stateMachine.TurnManager = this;
        MaxPlayersCount = 2;
        StartCoroutine(StartGame());

        _spawnBlocksController = spawnBlocksController;
        _environmentController = environmentController;
    }

    private void Update()
    {
        if(_stateMachine.ActualState is AnimalsEnvironmentSelectState)
            if(Input.GetKeyUp(KeyCode.A))
                WasSelectedOrSkipedAnimalsEnviroment();

        if(_stateMachine.ActualState is AnimalsSelectState)
            if (Input.GetKeyUp(KeyCode.S))
                WasAnimalsSkiped();
    }

    public IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1);
        _stateMachine.BlocksSelectState();
    }

    public void SubsribeOnStateMachine(Action<IState> action) => _stateMachine.OnStateChange += action;

    public void WasSpawnedBlock()
    {
        if(_spawnBlocksController.IsAnyBlockNotPlaced()) return;

        _stateMachine.AnimalsEnvironmentSelectState();
    }

    public void WasSelectedOrSkipedAnimalsEnviroment() =>
        _stateMachine.AnimalsSelectState();

    public void WasAnimalsSkiped() {
        int previousPlayerIndex = IndexActualPlayer;
        IndexActualPlayer++;
        if (IndexActualPlayer >= MaxPlayersCount)
            IndexActualPlayer = 0;

        OnRoundEnded?.Invoke(previousPlayerIndex, IndexActualPlayer);
        _stateMachine.BlocksSelectState();
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

    public Transform GetEnvironmentSpawn(int index) => _environmnetsSpawns[index];
}