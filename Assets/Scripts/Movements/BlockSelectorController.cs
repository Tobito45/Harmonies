using Harmonies.Cells;
using Harmonies.InitObjets;
using Harmonies.Score;
using Harmonies.States;
using System;
using Unity.Netcode;

namespace Harmonies.Selectors
{
    public class BlockSelectorController : ElementSelectorController
    {
        private GameBlock _blockInfo;
        private SpawnBlocksController _spawnBlocksController;
        private ScoreController _scoreController;
        public bool IsSpawnedInGame { get; private set; }

        public void Init(SpawnBlocksController spawnBlocksController, TurnManager turnManager, ScoreController scoreController)
        {
            _spawnBlocksController = spawnBlocksController;
            _turnManager = turnManager;
            _scoreController = scoreController;
        }

        public void Init() => InitClientRpc();

        [ClientRpc]
        public void InitClientRpc()
        {
            _blockInfo = GetComponent<GameBlock>();

            if (InitObjectsFactory.InitObjects.TryGetValue(GetType(), out Action<object> method))
                method(this);
            InitBase();
        }

        protected override void OnSpawnElementOnCell(GameCell gameCell)
        {
            gameCell.SpawnBlock(_blockInfo);
            if (gameCell.Node.IndexesCount == 1)
                _scoreController.CountFreeCells--;

            IsSpawnedInGame = true;
            int upd = ScoreBlockCalculator.CalculateChangesOnMap(gameCell.Node);
            (int score, int help) = ScoreBlockCalculator.GetNewScore(gameCell.Node);

            _scoreController.UpdateScore(score + upd);
            gameCell.HelperNumberScore = help;

            _spawnBlocksController.WasSpawnedBlock();
        }
        protected override void OnStatusChange(IState newState) => _unableInteraction = newState is not BlocksPlaceSelectState;
    }
}

