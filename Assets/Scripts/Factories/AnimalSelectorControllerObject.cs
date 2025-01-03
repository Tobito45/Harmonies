using Harmonies.Score;
using Harmonies.Selectors;
using System;

namespace Harmonies.InitObjets
{
    internal class AnimalSelectorControllerObject : InitObjectBase, IInitObject
    {
        private TurnManager _turnManager;
        private ScoreController _scoreController;

        public AnimalSelectorControllerObject(TurnManager turnManager, ScoreController scoreController)
        {
            _turnManager = turnManager;
            _scoreController = scoreController;
            MainType = typeof(AnimalSelectorController);
        }

        public void Init(object obj)
        {
            if (obj is not AnimalSelectorController animal)
                throw new Exception("Bad type action");

            animal.Init(_turnManager, _scoreController);
        }
    }
}