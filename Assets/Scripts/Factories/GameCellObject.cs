using Harmonies.Cells;
using Harmonies.Enviroment;
using Harmonies.Selectors;
using System;


namespace Harmonies.InitObjets
{
    internal class GameCellObject : InitObjectBase, IInitObject, IPredicateObject
    {
        private BoardSceneGenerator[] _boardSceneGenerator;
        private SpawnBlocksController _spawnBlocksController;
        private TurnManager _turnManager;

        public GameCellObject(BoardSceneGenerator[] boardSceneGenerator, TurnManager turnManager, SpawnBlocksController spawnBlocksController)
        {
            _boardSceneGenerator = boardSceneGenerator;
            _turnManager = turnManager;
            _spawnBlocksController = spawnBlocksController;
            MainType = typeof(GameCell);
        }

        public void Init(object obj)
        {
            if (obj is not (GameCell cell, int index, int i))
                throw new Exception("Bad type action");

            cell.Init(_boardSceneGenerator[i].BoardGraph.GetNodeByIndex(index), _turnManager, _spawnBlocksController);
        }

        public bool PredicateGameCell(object obj)
        {
            if (obj is not (GameCell cell, int index, int i))
                throw new Exception("Bad type predicate");

            return (_boardSceneGenerator[i].BoardGraph != null);
        }
    }
}