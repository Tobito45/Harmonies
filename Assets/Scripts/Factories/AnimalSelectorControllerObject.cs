using Harmonies.Conditions;
using Harmonies.Selectors;
using System;

namespace Harmonies.InitObjets
{
    internal class AnimalSelectorControllerObject : InitObjectBase, IInitObject
    {
        private TurnManager _turnManager;

        public AnimalSelectorControllerObject(TurnManager turnManager)
        {
            _turnManager = turnManager;
            MainType = typeof(AnimalSelectorController);
        }

        public void Init(object obj)
        {
            if (obj is not AnimalSelectorController animal)
                throw new Exception("Bad type action");

            animal.Init(_turnManager);
        }
    }
}