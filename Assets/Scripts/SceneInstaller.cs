using Harmonies.Enviroment;
using Harmonies.InitObjets;
using Harmonies.Score;
using Harmonies.Selectors;
using Harmonies.States;
using System.Linq;
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

    [SerializeField]
    private NetworkManagerUI _networkManagerUI;

    [SerializeField]
    private NetworkPlayersController _networkPlayersController;

    [SerializeField]
    private ScoreController _scoreController;

    public override void InstallBindings()
    {
        Container.Bind<StateMachine>().AsSingle().NonLazy();
        Container.Bind<TurnManager>().FromComponentInHierarchy(_turnManager).AsSingle();
        Container.Bind<SpawnBlocksController>().FromComponentInHierarchy(_spawnBlocksController).AsSingle();
        Container.Bind<EnvironmentController>().FromComponentInHierarchy(_environmentController).AsSingle();
        Container.Bind<NetworkManagerUI>().FromComponentInHierarchy(_networkManagerUI).AsSingle();
        Container.Bind<NetworkPlayersController>().FromComponentInHierarchy(_networkPlayersController).AsSingle();
        Container.Bind<ScoreController>().FromComponentInHierarchy(_scoreController).AsSingle();

        InitObjectsFactory.Init(_turnManager, _environmentController, 
            _spawnBlocksController, _turnManager.GetAllBoardSceneGenerators.ToArray(), _scoreController);

    }

    private void FindAllObjects()
    {
        if (_turnManager == null) _turnManager = FindObjectOfType<TurnManager>();
        if (_spawnBlocksController == null) _spawnBlocksController = FindObjectOfType<SpawnBlocksController>();
        if (_environmentController == null) _environmentController = FindObjectOfType<EnvironmentController>();
    }
}
