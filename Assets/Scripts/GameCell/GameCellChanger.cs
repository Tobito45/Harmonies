using Harmonies.Enums;
using Harmonies.Structures;

namespace Harmonies.Cells
{
    internal static class GameCellChanger
    {
        public static BlockCombinations GetType(BoardNode<BlockType> node)
        {
            switch (node.IndexesCount)
            {
                case 1:
                    switch (node.GetIndex(0))
                    {
                        case BlockType.Grass:
                            return BlockCombinations.Grass;
                        case BlockType.Wood:
                            return BlockCombinations.Wood;
                        case BlockType.Stone:
                            return BlockCombinations.Stone;
                        case BlockType.Building:
                            return BlockCombinations.Building;
                        case BlockType.Field:
                            return BlockCombinations.Field;
                        case BlockType.Water:
                            return BlockCombinations.Water;
                    }
                    break;
                case 2:
                    switch (node.GetIndex(1))
                    {
                        case BlockType.Grass:
                            return BlockCombinations.GrassWood;
                        case BlockType.Wood:
                            return BlockCombinations.DoubleWood;
                        case BlockType.Stone:
                            return BlockCombinations.DoubleStone;
                        case BlockType.Building:
                            switch(node.GetIndex(0))
                            {
                                case BlockType.Building:
                                    return BlockCombinations.BuildingBuilding;
                                case BlockType.Wood:
                                    return BlockCombinations.BuildingWood;
                                case BlockType.Stone:
                                    return BlockCombinations.BuildingStone;
                            }
                            break;
                    }
                    break;
                case 3:
                    switch (node.GetIndex(2))
                    {
                        case BlockType.Grass:
                            return BlockCombinations.DoubleWoodGrass;
                        case BlockType.Stone:
                            return BlockCombinations.TripleStone;
                    }
                    break;
            }
            return BlockCombinations.Grass;
        }


    }

    internal enum BlockCombinations
    {
        Stone,
        DoubleStone,
        TripleStone,
        BuildingStone,
        BuildingBuilding,
        Building,
        BuildingWood,
        Wood,
        DoubleWood,
        Grass,
        GrassWood,
        DoubleWoodGrass,
        Water,
        Field
    }
}
