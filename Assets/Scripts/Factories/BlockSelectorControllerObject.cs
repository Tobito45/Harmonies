using Harmonies.Selectors;
using System;

namespace Harmonies.InitObjets
{
    internal class BlockSelectorControllerObject : InitObjectBase, IInitObject
    {
        private SpawnBlocksController _spawnBlocksController;

        public BlockSelectorControllerObject(SpawnBlocksController spawnBlocksController)
        {
            _spawnBlocksController = spawnBlocksController;
            MainType = typeof(BlockSelectorController);
        }

        public void Init(object obj)
        {
            if (obj is not BlockSelectorController block)
                throw new Exception("Bad type action");

            block.Init(_spawnBlocksController);
        }
    }
}