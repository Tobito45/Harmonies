using Harmonies.Enums;
using Harmonies.Structures;
using System.Collections.Generic;
using UnityEngine;

namespace Harmonies.Score
{
    public static class ScoreBlockCalculator
    {
        public static (int, int) GetNewScore(BoardNode<BlockType> node)
        {
            return node.GetIndex(node.IndexesCount - 1) switch
            {
                BlockType.Grass => WoodScore(node),
                BlockType.Field => FieldScore(node),
                BlockType.Stone => StoneScore(node),
                BlockType.Building => BuildingScore(node),
                BlockType.Water => NewMethod(node),
                _ => (0, 0)
            };
        }
        public static int CalculateChangesOnMap(BoardNode<BlockType> node)
        {
            foreach (BoardNode<BlockType> neighBoard in node)
            {
                if (neighBoard == null || neighBoard.IndexesCount == 0)
                    continue;

                if (neighBoard.GetIndex(neighBoard.IndexesCount - 1) == BlockType.Building)
                {
                    int number = neighBoard.GameCell.HelperNumberScore;
                    bool was = HasAtLeastThreeOnes(number);
                    if (node.IndexesCount > 1)
                        number &= ~(1 << (int)node.GetIndex(node.IndexesCount - 2));
                    number |= (1 << (int)node.GetIndex(node.IndexesCount - 1));
                    bool newHas = HasAtLeastThreeOnes(number);
                    neighBoard.GameCell.HelperNumberScore = number;
                    if (was && !newHas)
                        return -5;
                    else if (!was && newHas)
                    {
                        Debug.Log(5);
                        return 5;
                    }
                    else
                        return 0;
                }
            }
            return 0;
        }

        private static (int, int) NewMethod(BoardNode<BlockType> node)
        {
            Dictionary<int, int> dict = new();
            var result = RecursiveReadingWater(node, new HashSet<BoardNode<BlockType>>(), 0, 1, dict);
            if (result.sum > ScoreController.WaterNumber)
            {
                int sub = result.sum - ScoreController.WaterNumber;
                ScoreController.WaterNumber = result.sum;
                return (sub, result.sum);
            }
            return (0, result.sum);
        }

        private static (int, int) BuildingScore(BoardNode<BlockType> node)
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

        private static (int, int) StoneScore(BoardNode<BlockType> node)
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

        private static (int, int) FieldScore(BoardNode<BlockType> node)
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

        private static (int, int) WoodScore(BoardNode<BlockType> node)
        {
            return node.IndexesCount switch
            {
                1 => (1, -1),
                2 => (3, -1),
                3 => (7, -1),
                _ => (0, 0),
            };
        }

        private static (int sum, int last) RecursiveReadingWater(BoardNode<BlockType> node, HashSet<BoardNode<BlockType>> already, int sum, int length, Dictionary<int, int> dict)
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

        private static int CalculateStone(BoardNode<BlockType> node)
        {
            return node.IndexesCount switch
            {
                1 => 1,
                2 => 2,
                3 => 4,
                _ => 0,
            };
        }

        private static int CalculateWater(int count)
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

        private static bool HasAtLeastThreeOnes(int number)
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
