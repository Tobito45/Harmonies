using Harmonies.Enviroment;
using Harmonies.InitObjets;
using Harmonies.Score;
using Harmonies.Score.AnimalCard;
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

    [SerializeField]
    private EndScoreUI _endScore;
    [SerializeField]
    private AnimalsCardsUI _animalCardUI;

    public override void InstallBindings()
    {
        Container.Bind<StateMachine>().AsSingle().NonLazy();
        Container.Bind<TurnManager>().FromComponentInHierarchy(_turnManager).AsSingle();
        Container.Bind<SpawnBlocksController>().FromComponentInHierarchy(_spawnBlocksController).AsSingle();
        Container.Bind<EnvironmentController>().FromComponentInHierarchy(_environmentController).AsSingle();
        Container.Bind<NetworkManagerUI>().FromComponentInHierarchy(_networkManagerUI).AsSingle();
        Container.Bind<NetworkPlayersController>().FromComponentInHierarchy(_networkPlayersController).AsSingle();
        Container.Bind<ScoreController>().FromComponentInHierarchy(_scoreController).AsSingle();
        Container.Bind<EndScoreUI>().FromComponentInHierarchy(_endScore).AsSingle();
        Container.Bind<AnimalsCardsUI>().FromComponentInHierarchy(_animalCardUI).AsSingle();

        InitObjectsFactory.Init(_turnManager, _environmentController, 
            _spawnBlocksController, _turnManager.GetAllBoardSceneGenerators.ToArray(), _scoreController, _animalCardUI);

    }

    private void OnDisable() => InitObjectsFactory.ClearDictionaries();
}
