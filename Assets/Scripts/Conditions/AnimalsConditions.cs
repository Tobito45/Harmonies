using Harmonies.Enums;
using Harmonies.Structures;


namespace Harmonies.Conditions
{
    internal class AnimalsConditions
    {
        public AnimalsConditions()
        {
            BaseConditions.AddToDictionary(ConditionName.First, FirstCondition);
        }

        private static bool FirstCondition(BoardNode<BlockType> node)
        {
            foreach (BoardNode<BlockType> neighbour in node)
            {
                if (neighbour == null) continue;

                if (node.IndexesCount == 1 && node.GetIndex(0) == BlockType.Field
                    && neighbour.IndexesCount == 1 && neighbour.GetIndex(0) == BlockType.Field)
                    return true;

                if (node.IndexesCount == 1 && node.GetIndex(0) == BlockType.Grass
                    && neighbour.IndexesCount == 1 && neighbour.GetIndex(0) == BlockType.Grass)
                    return true;

                if (node.IndexesCount == 1 && node.GetIndex(0) == BlockType.Stone
                    && neighbour.IndexesCount == 1 && neighbour.GetIndex(0) == BlockType.Stone)
                    return true;
            }
            return false;
        }
    }
}
