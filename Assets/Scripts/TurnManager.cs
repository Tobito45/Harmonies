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


    [Inject]
    public void Construct(StateMachine stateMachine, SpawnBlocksController spawnBlocksController, EnvironmentController environmentController)
    {
        _spawnBlocksController = spawnBlocksController;
        _environmentController = environmentController;

        _stateMachine = stateMachine;
        stateMachine.TurnManager = this;
        StartCoroutine(StartGame());
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

    public void WasAnimalsSkiped() => _stateMachine.BlocksSelectState();
    public void SpawnEnvironmentToPlayerZone() => _environmentController.CreatePlayerSelectableEnvironment();
    public void WasSelectedBlocksSelector() => _stateMachine.BlocksPlaceState();

    public bool IsAnyEnviroment() => _environmentController.IsAnyEnviroment();
}
