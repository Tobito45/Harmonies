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
            //BaseConditions.AddToDictionary("First", FirstCondition);
        }

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
    }
}
