using Harmonies.Enums;
using Harmonies.Structures;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace Harmonies.Selectors
{
    public class GameBlock : NetworkBehaviour
    {
        [field: SerializeField]
        public GameObject Prefab { get; private set; }

        [field: SerializeField]
        public BlockType Index { get; private set; }

        [ServerRpc(RequireOwnership = false)]
        public void DisableServerRpc() => GetComponent<NetworkObject>().Despawn();

        public int HelpNumber { get; set; }

        public (int, int) GetNewScore(BoardNode<BlockType> node)
        {
            if (Index == BlockType.Grass)
            {
                switch (node.IndexesCount)
                {
                    case 1:
                        return (1, -1);
                    case 2:
                        return (3, -1);
                    case 3:
                        return (7, -1);
                }
            }

            if (Index == BlockType.Field)
            {
                bool findedExists = false;
                List<BoardNode<BlockType>> blocksField = new();
                foreach (BoardNode<BlockType> neighBoard in node)
                {
                    if (neighBoard == null)
                        continue;

                    if (neighBoard.IndexesCount == 1 && neighBoard.GetIndex(0) == BlockType.Field)
                    {
                        if (neighBoard.GameCell.HelperNumberScore == 1)
                            findedExists = true;
                        else
                            blocksField.Add(neighBoard);
                    }
                }

                blocksField.ForEach((block) => block.GameCell.HelperNumberScore = 1);
                if (findedExists)
                    return (0, 1);
                else
                {
                    if (blocksField.Count > 0)
                        return (5, 1);
                    else
                        return (0, 0);
                }
            }

            if (Index == BlockType.Stone)
            {
                bool finded = false;
                List<BoardNode<BlockType>> blocksStone = new();
                int result = 0;
                foreach (BoardNode<BlockType> neighBoard in node)
                {
                    if (neighBoard == null)
                        continue;

                    if (neighBoard.IndexesCount > 0 && neighBoard.GetIndex(0) == BlockType.Stone)
                    {
                        if (neighBoard.GameCell.HelperNumberScore == 1)
                            finded = true;
                        else
                            blocksStone.Add(neighBoard);
                    }
                }

                blocksStone.ForEach((block) =>
                {
                    block.GameCell.HelperNumberScore = 1;
                    result += CalculateStone(block);
                });

                if (finded || blocksStone.Count > 0)
                {
                    result += CalculateStone(node);
                    node.GameCell.HelperNumberScore = 1;
                    return (result, 1);
                }
                else
                    return (0, 0);

            }

            if (Index == BlockType.Building)
            {
                //WoWaSGFB
                int number = 0b000000;

                if (node.IndexesCount > 1 && node.GetIndex(0) == BlockType.Building)
                    return (0, number);

                foreach (BoardNode<BlockType> neighBoard in node)
                {
                    if (neighBoard == null || neighBoard.IndexesCount == 0)
                        continue;

                    BlockType type = neighBoard.GetIndex(neighBoard.IndexesCount - 1);

                    number |= (1 << (int)type);
                }

                if (HasAtLeastThreeOnes(number))
                    return (5, number);
                else
                    return (0, number);
            }

            if (Index == BlockType.Water)
            {
                Dictionary<int, int> dict = new();
                var result = RecursiveReadingWater(node, new HashSet<BoardNode<BlockType>>(), 0, 1, dict);
                if(result.sum > ScoreController.WaterNumber)
                {
                    int sub = result.sum - ScoreController.WaterNumber;
                    ScoreController.WaterNumber = result.sum;
                    return(sub, result.sum);
                }
                return(0, result.sum);
            }

            return (0, 0);

        }


        public (int sum, int last) RecursiveReadingWater(BoardNode<BlockType> node, HashSet<BoardNode<BlockType>> already, int sum, int length, Dictionary<int, int> dict)
        {
            already.Add(node);
            List<(int sum, int last)> results = new();
            foreach (BoardNode<BlockType> neighBoard in node)
            {
                if (neighBoard == null || neighBoard.IndexesCount != 1 || already.Contains(neighBoard))
                    continue;

                if (neighBoard.GetIndex(0) == BlockType.Water)
                {
                    HashSet<BoardNode<BlockType>> newAlready = new HashSet<BoardNode<BlockType>>(already);
                    (int sum, int last) item = RecursiveReadingWater(neighBoard, newAlready, sum + CalculateWater(length + 1), length + 1, dict);
                    results.Add(item);
                }
            }
            if (results.Count == 0)
            {
                if (!dict.ContainsKey(node.Id))
                    dict.Add(node.Id, sum);
                else
                {
                    if (dict[node.Id] > sum)
                        dict[node.Id] = sum;
                }
                return (sum, node.Id);
            }
            else
            {
                int sumMax = int.MinValue;
                int lastId = -1;
                foreach (var res in results)
                {
                    int resSum = res.sum;
                    if (dict.ContainsKey(res.last))
                        resSum = dict[res.last];
                    if (lastId == res.last)
                    {
                        if (resSum < sumMax)
                            sumMax = resSum;
                    }
                    else
                    {
                        if (resSum > sumMax)
                        {
                            sumMax = resSum;
                            lastId = res.last;
                        }
                    }
                }
                return (sumMax, lastId);
            }

        }

        public int CalculateChangesOnMap(BoardNode<BlockType> node)
        {
            //if ((Index == BlockType.Building || Index == BlockType.Grass) && node.IndexesCount > 1)
            {
                foreach (BoardNode<BlockType> neighBoard in node)
                {
                    if (neighBoard == null || neighBoard.IndexesCount == 0)
                        continue;

                    if (neighBoard.GetIndex(neighBoard.IndexesCount - 1) == BlockType.Building)
                    {
                        int number = neighBoard.GameCell.HelperNumberScore;
                        bool was = HasAtLeastThreeOnes(number);
                        if(node.IndexesCount > 1) 
                            number &= ~(1 << (int)node.GetIndex(node.IndexesCount - 2));
                        number |= (1 << (int)Index);
                        bool newHas = HasAtLeastThreeOnes(number);
                        neighBoard.GameCell.HelperNumberScore = number;
                        if (was && !newHas)
                            return -5;
                        else if(!was && newHas)
                            return 5;
                        else
                            return 0;
                    }
                }
            }
            return 0;
        }


        private int CalculateStone(BoardNode<BlockType> node)
        {
            return node.IndexesCount switch
            {
                1 => 1,
                2 => 2,
                3 => 4,
                _ => 0,
            };
        }

        private int CalculateWater(int count)
        {
            return count switch
            {
                1 => 0,
                2 => 2,
                3 => 3,
                4 => 3,
                5 => 3,
                6 => 4,
                _ => 4,
            };
        }

        private bool HasAtLeastThreeOnes(int number)
        {
            int count = 0;

            for (int i = 0; i < 6; i++)
            {
                if ((number & (1 << i)) != 0)
                {
                    count++;
                    if (count >= 3)
                        return true;
                }
            }

            return false;
        }

    }

}
