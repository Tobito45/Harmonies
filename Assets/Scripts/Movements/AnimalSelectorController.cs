using Harmonies.Cells;
using Harmonies.Enviroment;
using Harmonies.Score;
using Harmonies.States;

namespace Harmonies.Selectors
{
    public class AnimalSelectorController : ElementSelectorController
    {
        private GameAnimalsController _gameAnimalsController;
        private ScoreController _scoreController;

        public void Init(TurnManager turnManager, ScoreController scoreController)
        {
            _turnManager = turnManager;
            _scoreController = scoreController;
            _gameAnimalsController = transform.parent.GetComponent<GameAnimalsController>();
            InitBase();
        }

        protected override void OnSpawnElementOnCell(GameCell gameCell)
        {
            gameCell.SpawnAnimal(_gameAnimalsController.GameAnimal);
            _scoreController.UpdateScore(_gameAnimalsController.GameAnimal.GetNewScore(_gameAnimalsController.Index));
            _gameAnimalsController.AnimalWasSelected();
        }

        public void SetNotInteractable() => _unableInteraction = true;

        protected override void OnStatusChange(IState newState) => _unableInteraction = newState is not AnimalsSelectState;
    }
}
