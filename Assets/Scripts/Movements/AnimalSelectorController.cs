using Harmonies.Enviroment;
using Harmonies.States;

namespace Harmonies.Selectors
{
    public class AnimalSelectorController : ElementSelectorController
    {
        private GameAnimalsController _gameAnimalsController;
        
        public void Init(TurnManager turnManager)
        {
            _turnManager = turnManager;
            _gameAnimalsController = transform.parent.GetComponent<GameAnimalsController>();
            InitBase();
        }

        protected override void OnSpawnElementOnCell(GameCell gameCell)
        {
            gameCell.SpawnAnimal(_gameAnimalsController.GameAnimal);
            _gameAnimalsController.AnimalWasSelected();
        }

        protected override void OnStatusChange(IState newState) => _unableInteraction = newState is not AnimalsSelectState;
    }
}
