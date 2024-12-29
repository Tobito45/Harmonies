using Harmonies.InitObjets;
using Harmonies.States;
using Harmonies.Structures;
using System;
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

        public void Init(SpawnBlocksController spawnBlocksController, TurnManager turnManager)
        {
            _spawnBlocksController = spawnBlocksController;
            _turnManager = turnManager;
        }

        public void Init() => InitClientRpc();

        [ClientRpc]
        public void InitClientRpc()
        {
            _blockInfo = GetComponent<GameBlock>();

            if (InitObjectsFactory.InitObject.TryGetValue(GetType(), out Action<object> method))
                method(this);
            InitBase();
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

