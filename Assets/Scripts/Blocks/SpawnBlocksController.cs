using Harmonies.Selectors;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;


namespace Harmonies.Selectors
{
    public class SpawnBlocksController : MonoBehaviour //not need be monobehaviour
    {
        private TurnManager _turnManager;

        private BlockSelectorController[] _alreadySpawnedBlocks;

        [Inject]
        public void Construct(TurnManager turnManager) => _turnManager = turnManager;

        public void SetAlreadySpawnedBlocks(BlockSelectorController[] spawned) =>
            _alreadySpawnedBlocks = spawned;

        public bool IsAnyBlockNotPlaced()
        {
            for (int i = 0; i < _alreadySpawnedBlocks.Length; i++)
                if (!_alreadySpawnedBlocks[i].IsSpawned)
                    return true;

            return false;
        }
        public void WasSpawnedBlock() => _turnManager.WasSpawnedBlock();
    }
}
