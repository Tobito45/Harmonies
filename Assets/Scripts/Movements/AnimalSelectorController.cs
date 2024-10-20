using Harmonies.Structures;
using UnityEngine;

namespace Harmonies.Selectors
{
    public class AnimalSelectorController : ElementSelectorController
    {
        [SerializeField]
        private string _conditionName;

        private GameAnimal _gameAnimal;
        
        protected override void Start()
        {
            _gameAnimal = transform.parent.GetComponent<GameAnimal>();
            base.Start();
        }

        protected override void OnSpawnElementOnCell(GameCell gameCell)
        {
            gameCell.SpawnAnimal(this);
            _gameAnimal.AnimalWasSelected();
        }

        public bool IsConditionToSpawn(BoardNode node) => AnimalsConditions.GetConditionFunction(_conditionName)(node);

        public override bool SelectExceptions(BoardNode node) => node.IndexesCount == 0 || !IsConditionToSpawn(node);
    }
}
