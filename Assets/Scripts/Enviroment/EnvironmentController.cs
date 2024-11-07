using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Harmonies.Environment
{
    public class EnvironmentController : MonoBehaviour
    {
        private TurnManager _turnManager;

        [SerializeField]
        private Transform[] _spawnPoints;

        [SerializeField]
        private GameObject _prefabEnviroment;
        private GameAnimal[] _environments = new GameAnimal[4];

        [Inject]
        public void Construct(TurnManager turnManager) => _turnManager = turnManager;

        public void CreatePlayerSelectableEnvironment()
        {
            for (int i = 0; i < _environments.Length; i++)
            {
                if (_environments[i] == null)
                {
                    GameObject obj = Instantiate(_prefabEnviroment, _spawnPoints[i].position, _prefabEnviroment.transform.rotation);
                    obj.SetActive(true);
                    GameAnimal gameAnimal = obj.GetComponent<GameAnimal>();
                    gameAnimal.Init(this, _turnManager);
                    _environments[i] = gameAnimal;

                    _turnManager.WasSelectedOrSkipedAnimalsEnviroment();
                    return;
                }
            }
        }

        public void DeletePlayerSelectableEnviroment(GameAnimal animal)
        {
            for (int i = 0; i < _environments.Length; i++)
            {
                if (_environments[i] == animal)
                    _environments[i] = null;

                Destroy(animal.gameObject);
            }

        }

        public bool CanCreate()
        {
            for (int i = 0; i < _environments.Length; i++)
                if (_environments[i] == null) return true;

            return false;
        }

        public bool IsAnyEnviroment()
        {
            for (int i = 0; i < _environments.Length; i++)
                if (_environments[i] != null) return true;

            return false;
        }
    }
}
