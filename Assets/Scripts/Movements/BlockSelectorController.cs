using Harmonies.States;
using Harmonies.Structures;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Harmonies.Selectors
{
    public class BlockSelectorController : ElementSelectorController
    {
        private GameBlock _blockInfo;
        private SpawnBlocksController _spawnBlocksController;
        public bool IsSpawned { get; private set; }

        public void Init() => InitClientRpc();

        [ClientRpc]
        public void InitClientRpc()
        {
            _blockInfo = GetComponent<GameBlock>();
            _spawnBlocksController = FindObjectOfType<SpawnBlocksController>();//spawnBlocksController;
            base.Start();
        }

        protected override void OnSpawnElementOnCell(GameCell gameCell)
        {
            gameCell.SpawnBlock(_blockInfo);
            IsSpawned = true;
            _spawnBlocksController.WasSpawnedBlock();
        }
        public override bool SelectExceptions(BoardNode node) => false;

        protected override void OnStatusChange(IState newState) => _unableInteraction = newState is not BlocksPlaceSelectState;
    }
}

