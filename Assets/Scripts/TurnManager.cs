using Harmonies.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _spawnObjects; //to controller

    [SerializeField]
    private EnvironmentController _environmentController;

    private StateMachine _stateMachine;


    [Inject]
    public void Construct(StateMachine stateMachine)
    {
        if(_environmentController == null)
            _environmentController = FindObjectOfType<EnvironmentController>();
        
        _stateMachine = stateMachine;
        stateMachine.TurnManager = this;
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.A))
            _stateMachine.BlocksSelectState();

        if (Input.GetKeyUp(KeyCode.S))
            _stateMachine.AnimalsSelectState();

        if (Input.GetKeyUp(KeyCode.D))
            _stateMachine.AnimalsEnvironmentSelectState();
    }

    public void SpawnBlocks()
    {
        for(int i = 0; i < 3; i++)
        {
           GameObject obj = Instantiate(_spawnObjects[i]);
           obj.SetActive(true);
        }
    }

    public void SpawnEnvironmentToPlayerZone() => _environmentController.CreatePlayerSelectableEnvironment();
}
