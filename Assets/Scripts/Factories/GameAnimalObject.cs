using Harmonies.Enviroment;
using Harmonies.Score.AnimalCard;
using System;

namespace Harmonies.InitObjets
{
    internal class GameAnimalObject : InitObjectBase, IInitObject
    {
        private EnvironmentController _environmentController;
        private TurnManager _turnManager;
        private AnimalsCardsUI _animalsCardsUI;

        public GameAnimalObject(EnvironmentController environmentController, TurnManager turnManager, AnimalsCardsUI animalsCardsUI)
        {
            _environmentController = environmentController;
            _turnManager = turnManager;
            _animalsCardsUI = animalsCardsUI;
            MainType = typeof(GameAnimalsController);
        }

        public void Init(object obj)
        {
            if (obj is not GameAnimalsController animal)
                throw new Exception("Bad type action");

            animal.Init(_environmentController, _turnManager, _animalsCardsUI);
        }
    }
}

