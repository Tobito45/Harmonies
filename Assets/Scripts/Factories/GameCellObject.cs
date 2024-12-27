using System;


namespace Harmonies.InitObjets
{
    internal class GameCellObject : InitObjectBase, IInitObject, IPredicateObject
    {
        private BoardSceneGenerator[] _boardSceneGenerator;
        private TurnManager _turnManager;

        public GameCellObject(BoardSceneGenerator[] boardSceneGenerator, TurnManager turnManager)
        {
            _boardSceneGenerator = boardSceneGenerator;
            _turnManager = turnManager;
            MainType = typeof(GameCell);
        }

        public void Init(object obj)
        {
            if (obj is not (GameCell cell, int index, int i))
                throw new Exception("Bad type action");

            cell.Init(_boardSceneGenerator[i].BoardGraph.GetNodeByIndex(index), _turnManager);
        }

        public bool PredicateGameCell(object obj)
        {
            if (obj is not (GameCell cell, int index, int i))
                throw new Exception("Bad type predicate");

            return (_boardSceneGenerator[i].BoardGraph != null);
        }
    }
}