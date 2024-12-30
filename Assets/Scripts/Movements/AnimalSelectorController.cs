using Harmonies.Conditions;
using Harmonies.States;
using Harmonies.Structures;
using UnityEngine;

namespace Harmonies.Selectors
{
    public class AnimalSelectorController : ElementSelectorController
    {
        private GameAnimal _gameAnimal;
        
        public void Init(TurnManager turnManager)
        {
            _turnManager = turnManager;
            _gameAnimal = transform.parent.GetComponent<GameAnimal>();
            InitBase();
        }

        protected override void OnSpawnElementOnCell(GameCell gameCell)
        {
            gameCell.SpawnAnimal(this);
            _gameAnimal.AnimalWasSelected();
        }

        protected override void OnStatusChange(IState newState) => _unableInteraction = newState is not AnimalsSelectState;
    }
}
