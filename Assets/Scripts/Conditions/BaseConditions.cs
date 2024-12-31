using Harmonies.Enums;
using Harmonies.Structures;
using System;
using System.Collections.Generic;

namespace Harmonies.Conditions
{
    public static class BaseConditions
    {
        //true if its correct
        static Dictionary<ConditionName, Func<BoardNode<BlockType>, bool>> _ñonditionsdDictionary = new();
 
        private static BlocksConditions _blocksConditions = new BlocksConditions();
        private static AnimalsConditions _animalsConditions = new AnimalsConditions();
        public static Func<BoardNode<BlockType>, bool> GetConditionFunction(ConditionName name) => _ñonditionsdDictionary[name];
        internal static void AddToDictionary(ConditionName key, Func<BoardNode<BlockType>, bool> func) => _ñonditionsdDictionary.Add(key, func);
    }

    [Flags]
    public enum ConditionName
    {
        None = 0,
        Empty = 1 << 0,
        OneStone = 1 << 1,
        TwoStone = 1 << 2,
        OneWood = 1 << 3,
        TwoWood = 1 << 4,
        OneBuilding = 1 << 5,
        Boar = 1 << 6,
        Bear = 1 << 7,
        Tucan = 1 << 8,
        Crocodile = 1 << 9,
        Fox = 1 << 10,
        Meerkat = 1 << 11,
    }
}