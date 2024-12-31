using Harmonies.Enviroment;
using Harmonies.Selectors;
using System;
using UnityEngine;

namespace Harmonies.InitObjets
{
    internal class BlockSelectorControllerObject : InitObjectBase, IInitObject, IPredicateObject
    {
        private SpawnBlocksController _spawnBlocksController;
        private TurnManager _turnManager;

        public BlockSelectorControllerObject(SpawnBlocksController spawnBlocksController, TurnManager turnManager)
        {
            _spawnBlocksController = spawnBlocksController;
            _turnManager = turnManager;
            MainType = typeof(BlockSelectorController);
        }

        public void Init(object obj)
        {
            if (obj is not BlockSelectorController block)
                throw new Exception("Bad type action");

            block.Init(_spawnBlocksController, _turnManager);
        }

        public bool PredicateGameCell(object obj)
        {
            if (obj is not (GameAnimalsController[][] environment, int index, int actual))
                throw new Exception("Bad type predicate");

            return (environment[index][actual] != null);
        }
    }
}