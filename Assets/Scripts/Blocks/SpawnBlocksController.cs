using Harmonies.Selectors;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using Zenject;


namespace Harmonies.Selectors
{
    public class SpawnBlocksController : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _spawnBlocksPrefabs;

        [SerializeField]
        private GameObject[] _spawnedBlocksPrefabs;

        private TurnManager _turnManager;

        private BlockSelectorController[] _alreadySpawnedBlocks;

        [Inject]
        public void Construct(TurnManager turnManager) => _turnManager = turnManager;

        public void SetAlreadySpawnedBlocks(BlockSelectorController[] spawned) => _alreadySpawnedBlocks = spawned;

        public bool IsAnyBlockNotPlaced()
        {
            for (int i = 0; i < _alreadySpawnedBlocks.Length; i++)
                if (!_alreadySpawnedBlocks[i].IsSpawnedInGame)
                    return true;

            return false;
        }
        public void WasSpawnedBlock() => _turnManager.WasSpawnedBlock();

        public GameObject GetRandomSpawnBlock => _spawnBlocksPrefabs[Random.Range(0, _spawnBlocksPrefabs.Length)];     
        public GameObject GetSpawnedBlock(int index) => _spawnedBlocksPrefabs[index];     
    }
}
