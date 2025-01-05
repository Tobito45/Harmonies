using Harmonies.Enums;
using Harmonies.InitObjets;
using Harmonies.Selectors;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Harmonies.Enviroment
{
    public class EnvironmentController : NetworkBehaviour
    {
        private const int MAX_PLAYERS = 4; //TODO: other way
        private TurnManager _turnManager;

        [SerializeField]
        private GameObject[] _prefabEnviroments;
        [SerializeField]
        private GameObject[] _prefabAnimalsSpawn;
        [SerializeField]
        private GameObject[] _prefabEnviromentsSelectSpawn;

        private GameAnimalsController[][] _environments;

        [Inject]
        public void Construct(TurnManager turnManager)
        {
            _turnManager = turnManager;
            _environments = new GameAnimalsController[MAX_PLAYERS][];
            for (int i = 0; i < _environments.Length; i++)
                _environments[i] = new GameAnimalsController[MAX_PLAYERS];
        }

        public void CreatePlayerSelectableEnvironment(AnimalType animal)
        {
            for (int i = 0; i < _environments.Length; i++)
            {
                if (_environments[i][_turnManager.GetActualPlayerNumber] == null)
                {
                    CreateEnveromentServerRpc(i, _turnManager.GetActualPlayerNumber, (int)animal);
                    StartCoroutine(InitObjectsFactory.WaitForCallbackWithPredicate(typeof(BlockSelectorController), 
                            (_environments, i, _turnManager.GetActualPlayerNumber), () =>
                            {
                                _turnManager.WasSelectedOrSkipedAnimalsEnviroment();
                            }));
                    return;
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void CreateEnveromentServerRpc(int index, int actual, int animalIndex)
        {
            GameObject obj = Instantiate(_prefabEnviroments[animalIndex],
                _turnManager.GetActualPlayerInfo.GetEnvironmentSpawn(index).position,
                _turnManager.GetActualPlayerInfo.GetEnvironmentSpawn(index).rotation);// _prefabEnviroments[randomIndex].transform.rotation);
            obj.SetActive(true);
            obj.GetComponent<NetworkObject>().Spawn();
            GameAnimalsController gameAnimal = obj.GetComponent<GameAnimalsController>();
            gameAnimal.Init();
            SyncForClientRpc(index, actual, obj.GetComponent<NetworkObject>().NetworkObjectId);
        }

        [ClientRpc]
        private void SyncForClientRpc(int index, int actual, ulong networkIndex)
        {
            NetworkTools.FindNetworkObjectAndMakeAction(networkIndex,
                (networkObject) => _environments[index][actual] = networkObject.gameObject.GetComponent<GameAnimalsController>());
        }

        public void DeletePlayerSelectableEnviroment(GameAnimalsController animal)
        {
            for (int i = 0; i < _environments.Length; i++)
            {
                if (_environments[i][_turnManager.GetActualPlayerNumber] == animal)
                {
                    _environments[i][_turnManager.GetActualPlayerNumber] = null;
                    DestroyServerRpc(animal.GetComponent<NetworkObject>().NetworkObjectId);
                    break;
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void DestroyServerRpc(ulong networkIndex)
        {
            NetworkTools.FindNetworkObjectAndMakeAction(networkIndex,
                (networkObject) => networkObject.Despawn());
        }

        public bool CanCreate()
        {
            for (int i = 0; i < _environments.Length; i++)
                if (_environments[i][_turnManager.GetActualPlayerNumber] == null) return true;

            return false;
        }

        public bool IsAnyEnviroment()
        {
            for (int i = 0; i < _environments.Length; i++)
                if (_environments[i][_turnManager.GetActualPlayerNumber] != null) return true;

            return false;
        }

        public GameObject GetAnimalByIndex(int index) => _prefabAnimalsSpawn[index];
    }
}
