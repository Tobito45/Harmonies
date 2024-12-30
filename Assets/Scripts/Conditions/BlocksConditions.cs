using Harmonies.Enums;
using Harmonies.Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harmonies.Conditions
{
    internal class BlocksConditions
    {
        public BlocksConditions()
        {
            BaseConditions.AddToDictionary(ConditionName.Empty, EmptyCondition);
            BaseConditions.AddToDictionary(ConditionName.OneStone, OneStoneCondition);
            BaseConditions.AddToDictionary(ConditionName.TwoStone, TwoStoneCondition);
            BaseConditions.AddToDictionary(ConditionName.OneWood, OneWoodCondition);
            BaseConditions.AddToDictionary(ConditionName.TwoWood, TwoWoodCondition);
            BaseConditions.AddToDictionary(ConditionName.OneBuilding, OneBuildingCondition);
        }

        private static bool EmptyCondition(BoardNode<BlockType> node) => node.IndexesCount == 0;
        private static bool OneStoneCondition(BoardNode<BlockType> node) => node.IndexesCount == 1 && node.GetIndex(0) == BlockType.Stone;
        private static bool TwoStoneCondition(BoardNode<BlockType> node) => node.IndexesCount == 2 && node.GetIndex(0) == BlockType.Stone && node.GetIndex(1) == BlockType.Stone;
        private static bool OneWoodCondition(BoardNode<BlockType> node) => node.IndexesCount == 1 && node.GetIndex(0) == BlockType.Wood;
        private static bool TwoWoodCondition(BoardNode<BlockType> node) => node.IndexesCount == 2 && node.GetIndex(0) == BlockType.Wood && node.GetIndex(1) == BlockType.Wood;
        private static bool OneBuildingCondition(BoardNode<BlockType> node) => node.IndexesCount == 1 && node.GetIndex(0) == BlockType.Building;

    }
}
