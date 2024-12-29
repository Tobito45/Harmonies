using Harmonies.Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Harmonies.Conditions
{
    public static class BaseConditions
    {
        //true if its correct
        static Dictionary<string, Func<BoardNode, bool>> _ñonditionsdDictionary = new();
 
        private static BlocksConditions _blocksConditions = new BlocksConditions();
        private static AnimalsConditions _animalsConditions = new AnimalsConditions();
        public static Func<BoardNode, bool> GetConditionFunction(string name) => _ñonditionsdDictionary[name];
        internal static void AddToDictionary(string key, Func<BoardNode, bool> func) => _ñonditionsdDictionary.Add(key, func);
    }
}