using Harmonies.Environment;
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
            MainType = typeof(GameAnimal);
        }

        public void Init(object obj)
        {
            if (obj is not GameAnimal animal)
                throw new Exception("Bad type action");

            animal.Init(_environmentController, _turnManager);
        }
    }
}

