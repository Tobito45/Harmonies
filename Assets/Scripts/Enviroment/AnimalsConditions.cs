using Harmonies.Structures;
using System;
using System.Collections.Generic;


namespace Harmonies.Environment
{
    public static class AnimalsConditions
    {
        //true if its correct
        private static Dictionary<string, Func<BoardNode, bool>> _ñonditionsdDictionary = new Dictionary<string, Func<BoardNode, bool>>()
        {
            { "First", FirstCondition }
        };

        private static bool FirstCondition(BoardNode node)
        {
            foreach (BoardNode neighbour in node)
            {
                if (neighbour == null) continue;

                if (node.IndexesCount == 1 && node.GetIndex(0) == 0
                    && neighbour.IndexesCount == 1 && neighbour.GetIndex(0) == 0)
                    return true;

                if (node.IndexesCount == 1 && node.GetIndex(0) == 1
                    && neighbour.IndexesCount == 1 && neighbour.GetIndex(0) == 1)
                    return true;

                if (node.IndexesCount == 1 && node.GetIndex(0) == 2
                    && neighbour.IndexesCount == 1 && neighbour.GetIndex(0) == 2)
                    return true;
            }
            return false;
        }

        public static Func<BoardNode, bool> GetConditionFunction(string name) => _ñonditionsdDictionary[name];

    }
}
