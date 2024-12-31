using Harmonies.Enviroment;
using System;

namespace Harmonies.InitObjets
{
    internal class GameAnimalObject : InitObjectBase, IInitObject
    {
        private EnvironmentController _environmentController;
        private TurnManager _turnManager;

        public GameAnimalObject(EnvironmentController environmentController, TurnManager turnManager)
        {
            _environmentController = environmentController;
            _turnManager = turnManager;
            MainType = typeof(GameAnimalsController);
        }

        public void Init(object obj)
        {
            if (obj is not GameAnimalsController animal)
                throw new Exception("Bad type action");

            animal.Init(_environmentController, _turnManager);
        }
    }
}

