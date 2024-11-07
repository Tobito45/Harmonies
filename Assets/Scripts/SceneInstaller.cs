using Harmonies.Environment;
using Harmonies.Selectors;
using Harmonies.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField]
    private TurnManager _turnManager;

    [SerializeField]
    private SpawnBlocksController _spawnBlocksController;

    [SerializeField]
    private EnvironmentController _environmentController;

    public override void InstallBindings()
    {
        Container.Bind<StateMachine>().AsSingle().NonLazy();
        Container.Bind<TurnManager>().FromComponentInHierarchy(_turnManager).AsSingle();
        Container.Bind<SpawnBlocksController>().FromComponentInHierarchy(_spawnBlocksController).AsSingle();
        Container.Bind<EnvironmentController>().FromComponentInHierarchy(_environmentController).AsSingle();
    }

    private void FindAllObjects()
    {
        if (_turnManager == null) _turnManager = FindObjectOfType<TurnManager>();
        if (_spawnBlocksController == null) _spawnBlocksController = FindObjectOfType<SpawnBlocksController>();
        if (_environmentController == null) _environmentController = FindObjectOfType<EnvironmentController>();
    }
}
