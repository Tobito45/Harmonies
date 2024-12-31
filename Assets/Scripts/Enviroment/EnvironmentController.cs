using Harmonies.InitObjets;
using Harmonies.Selectors;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Harmonies.Enviroment
{
    public class EnvironmentController : NetworkBehaviour
    {
        private TurnManager _turnManager;

        [SerializeField]
        private GameObject[] _prefabEnviroments;
        [SerializeField]
        private GameObject[] _prefabAnimalsSpawn;

        private GameAnimalsController[][] _environments;

        [Inject]
        public void Construct(TurnManager turnManager)
        {
            _turnManager = turnManager;
            _environments = new GameAnimalsController[4][];
            for (int i = 0; i < _environments.Length; i++)
                _environments[i] = new GameAnimalsController[_turnManager.MaxPlayersCount];
        }

        public void CreatePlayerSelectableEnvironment()
        {
            for (int i = 0; i < _environments.Length; i++)
            {
                if (_environments[i][_turnManager.IndexActualPlayer] == null)
                {
                    CreateEnveromentServerRpc(i, _turnManager.IndexActualPlayer);
                    InitObjectsFactory.WaitForCallbackWithPredicate(typeof(BlockSelectorController), 
                            (_environments, i, _turnManager.IndexActualPlayer), () =>
                           _turnManager.WasSelectedOrSkipedAnimalsEnviroment());
                    return;
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void CreateEnveromentServerRpc(int index, int actual)
        {
            int randomIndex = Random.Range(0, _prefabEnviroments.Length);
            GameObject obj = Instantiate(_prefabEnviroments[randomIndex],
                _turnManager.GetActualPlayerInfo.GetEnvironmentSpawn(index).position,
                _prefabEnviroments[randomIndex].transform.rotation);
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
                if (_environments[i][_turnManager.IndexActualPlayer] == animal)
                {
                    _environments[i][_turnManager.IndexActualPlayer] = null;
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
                if (_environments[i][_turnManager.IndexActualPlayer] == null) return true;

            return false;
        }

        public bool IsAnyEnviroment()
        {
            for (int i = 0; i < _environments.Length; i++)
                if (_environments[i][_turnManager.IndexActualPlayer] != null) return true;

            return false;
        }

        public GameObject GetAnimalByIndex(int index) => _prefabAnimalsSpawn[index];
    }
}
