using Harmonies.Enviroment;
using System;

namespace Harmonies.InitObjets
{
    internal class EnvironmentSelectObject : InitObjectBase, IInitObject, IPredicateObject
    {
        private EnvironmentController _environmentController;
        private TurnManager _turnManager;

        public EnvironmentSelectObject(EnvironmentController environmentController, TurnManager turnManager)
        {
            _environmentController = environmentController;
            _turnManager = turnManager;
            MainType = typeof(EnvironmentSelect);
        }

        public void Init(object obj)
        {
            if (obj is not EnvironmentSelect block)
                throw new Exception("Bad type action");

            block.Init(_turnManager, _environmentController);
        }

        public bool PredicateGameCell(object obj)
        {
            if (obj is not (GameAnimalsController[][] environment, int index, int actual))
                throw new Exception("Bad type predicate");

            return (environment[index][actual] != null);
        }
    }
}