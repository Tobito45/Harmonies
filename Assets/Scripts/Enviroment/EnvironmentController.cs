using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Harmonies.Environment
{
    public class EnvironmentController : NetworkBehaviour
    {
        private TurnManager _turnManager;

        [SerializeField]
        private GameObject _prefabEnviroment;
        private GameAnimal[][] _environments;

        [Inject]
        public void Construct(TurnManager turnManager)
        {
            _turnManager = turnManager;
            _environments = new GameAnimal[4][];
            for (int i = 0; i < _environments.Length; i++)
                _environments[i] = new GameAnimal[_turnManager.MaxPlayersCount];
        }

        public void CreatePlayerSelectableEnvironment()
        {
            if (!IsOwner) return;
            
            for (int i = 0; i < _environments.Length; i++)
            {
                if (_environments[i][_turnManager.IndexActualPlayer] == null)
                {
                    GameObject obj = Instantiate(_prefabEnviroment, _turnManager.GetActualPlayerInfo().GetEnvironmentSpawn(i).position, _prefabEnviroment.transform.rotation);
                    obj.SetActive(true);
                    obj.GetComponent<NetworkObject>().Spawn();
                    GameAnimal gameAnimal = obj.GetComponent<GameAnimal>();
                    gameAnimal.Init(this, _turnManager);
                    _environments[i][_turnManager.IndexActualPlayer] = gameAnimal;

                    _turnManager.WasSelectedOrSkipedAnimalsEnviroment();
                    return;
                }
            }
        }

        public void DeletePlayerSelectableEnviroment(GameAnimal animal)
        {
            for (int i = 0; i < _environments.Length; i++)
            {
                if (_environments[i][_turnManager.IndexActualPlayer] == animal)
                    _environments[i][_turnManager.IndexActualPlayer] = null;

                Destroy(animal.gameObject);
            }

        }

        public bool CanCreate()
        {
            for (int i = 0; i < _environments.Length; i++)
                if (_environments[i][_turnManager.IndexActualPlayer] == null) return true;

            return false;
        }

        public bool IsAnyEnviroment()
        {
            for (int i = 0; i < _environments.Length; i++)
                if (_environments[i][_turnManager.IndexActualPlayer] != null) return true;

            return false;
        }
    }
}
