using Harmonies.Enums;
using Harmonies.Structures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harmonies.Cells
{
    internal static class GameCellChanger
    {
        public static BlockCombinations GetType(BoardNode<BlockType> node)
        {
            switch(node.IndexesCount)
            {
                case 1:
                    switch(node.GetIndex(0))
                    {
                        case BlockType.Grass:
                            return BlockCombinations.Grass;
                        case BlockType.Wood:
                            return BlockCombinations.Tree;
                        case BlockType.Stone:
                            return BlockCombinations.Stone;
                    }
                    break;
            }
            return BlockCombinations.DoubleStone;
        }


    }

    internal enum BlockCombinations
    {
        Grass,
        Tree,
        Stone,
        DoubleStone
    }
}
