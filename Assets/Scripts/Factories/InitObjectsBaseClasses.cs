using System;

namespace Harmonies.InitObjets
{
    internal interface IInitObject
    {
        public void Init(object obj);
    }

    internal interface IPredicateObject
    {
        public bool PredicateGameCell(object obj);
    }

    internal abstract class InitObjectBase
    {
        public Type MainType { get; protected set; }
    }
}