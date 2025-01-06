using Harmonies.Enums;
using Harmonies.Structures;
using UnityEngine;


namespace Harmonies.Conditions
{
    internal class AnimalsConditions
    {
        public AnimalsConditions()
        {
            BaseConditions.AddToDictionary(ConditionName.Boar, BoarCondition);
            BaseConditions.AddToDictionary(ConditionName.Bear, BearCondition);
            BaseConditions.AddToDictionary(ConditionName.Fox, FoxCondition);
            BaseConditions.AddToDictionary(ConditionName.Meerkat, MeerKatCondition);
            BaseConditions.AddToDictionary(ConditionName.Crocodile, CrocodileCondition);
            BaseConditions.AddToDictionary(ConditionName.Tucan, TucanCondition);
        }

        private static bool BoarCondition(BoardNode<BlockType> node)
        {
            Debug.Log("??21");
            if (node.IndexesCount == 2 && node.GetIndex(1) == BlockType.Building)
            {
                foreach (BoardNode<BlockType> neighbour in node)
                {
                    if (neighbour == null) continue;

                    if (neighbour.IndexesCount == 2 && neighbour.GetIndex(1) == BlockType.Grass)
                        return true;
                }
            }
            return false;
        }

        private static bool BearCondition(BoardNode<BlockType> node)
        {
            if (node.IndexesCount == 1 && node.GetIndex(0) == BlockType.Grass)
            {
                for(int i = 1; i < node.GetMaxNeighbours + 1; i ++)
                {
                    BoardNode<BlockType> first = node.GetNode(i);
                    BoardNode<BlockType> second = null;    
                    if(i + 1 > node.GetMaxNeighbours)
                        second = node.GetNode(1);
                    else
                        second = node.GetNode(i + 1);

                    if (first == null || second == null)
                        continue;

                    if (first.IndexesCount == 2 && second.IndexesCount == 2 &&
                        first.GetIndex(1) == BlockType.Stone && second.GetIndex(1) == BlockType.Stone)
                        return true;
                }
            }
            return false;
        }

        private static bool TucanCondition(BoardNode<BlockType> node)
        {
            if (node.IndexesCount == 2 && node.GetIndex(1) == BlockType.Grass)
            {
                for (int i = 1; i < node.GetMaxNeighbours + 1; i++)
                {
                    BoardNode<BlockType> first = node.GetNode(i);
                    BoardNode<BlockType> second = null;
                    if (i + 1 > node.GetMaxNeighbours)
                        second = node.GetNode(1);
                    else
                        second = node.GetNode(i + 1);

                    if (first == null || second == null)
                        continue;

                    if (first.IndexesCount == 1 && second.IndexesCount == 1 &&
                        first.GetIndex(0) == BlockType.Water && second.GetIndex(0) == BlockType.Water)
                        return true;
                }
            }
            return false;
        }
        private static bool CrocodileCondition(BoardNode<BlockType> node)
        {
            if (node.IndexesCount == 1 && node.GetIndex(0) == BlockType.Water)
            {
                for (int i = 1; i < node.GetMaxNeighbours + 1; i++)
                {
                    BoardNode<BlockType> first = node.GetNode(i);
                    BoardNode<BlockType> second = first.GetNode(i);

                    if (first == null || second == null)
                        continue;

                    if (first.IndexesCount == 1 && second.IndexesCount == 3 &&
                       first.GetIndex(0) == BlockType.Water && second.GetIndex(2) == BlockType.Grass)
                        return true;
                }
            }
            return false;
        }

        private static bool FoxCondition(BoardNode<BlockType> node)
        {
            if (node.IndexesCount == 1 && node.GetIndex(0) == BlockType.Stone)
            {
                for (int i = 1; i < node.GetMaxNeighbours + 1; i++)
                {
                    BoardNode<BlockType> first = node.GetNode(i);
                    if (first == null)
                        continue;

                    BoardNode<BlockType> second = first.GetNode(i);

                    if (second == null)
                        continue;

                    if (first.IndexesCount == 1 && second.IndexesCount == 1 &&
                       first.GetIndex(0) == BlockType.Stone && second.GetIndex(0) == BlockType.Field)
                        return true;
                }
            }
            return false;
        }

        private static bool MeerKatCondition(BoardNode<BlockType> node)
        {
            if (node.IndexesCount == 1 && node.GetIndex(0) == BlockType.Stone)
            {
                foreach (BoardNode<BlockType> neighbour in node)
                {
                    if (neighbour == null) continue;

                    if (neighbour.IndexesCount == 1 && neighbour.GetIndex(0) == BlockType.Field)
                        return true;
                }
            }
            return false;
        }
    }
}
