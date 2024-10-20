using Harmonies.Structures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harmonies.Selectors
{
    public class BlockSelectorController : ElementSelectorController
    {
        private GameBlock blockInfo;

        private void Start()
        {
            blockInfo = GetComponent<GameBlock>();
            base.Start();
        }

        protected override void OnSpawnElementOnCell(GameCell gameCell) => gameCell.SpawnBlock(blockInfo);
        public override bool SelectExceptions(BoardNode node) => false;
    }
}

