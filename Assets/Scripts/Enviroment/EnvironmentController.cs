using Harmonies.Enums;
using Harmonies.InitObjets;
using Harmonies.ScroptableObjects;
using Harmonies.Selectors;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Harmonies.Enviroment
{
    public class EnvironmentController : NetworkBehaviour
    {
        private const int MAX_PLAYERS = 4; //TODO: other way
        private TurnManager _turnManager;

        //can be one size
        private GameAnimalsController[][] _environments;
        private EnvironmentSelect[] _eviromentsSelect;

        private EnviromentDataConfig _enviromentDataConfig;

        [Inject]
        public void Construct(TurnManager turnManager)
        {
            _turnManager = turnManager;
            _environments = new GameAnimalsController[MAX_PLAYERS][];
            for (int i = 0; i < _environments.Length; i++)
                _environments[i] = new GameAnimalsController[MAX_PLAYERS];

            _eviromentsSelect = new EnvironmentSelect[_turnManager.GetPlayerInfo(0).GetEnvironmentSelectSpawnCount];
            _enviromentDataConfig = Resources.Load<EnviromentDataConfig>("EnviromentDataConfig");
        }

        public void CreatePlayerSelectedEnvironment(AnimalType animal, EnvironmentSelect environmentSelect)
        {
            WasSelectedEnviroment(environmentSelect);

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

        private void WasSelectedEnviroment(EnvironmentSelect environmentSelect)
        {
            for(int i = 0; i < _eviromentsSelect.Length; i++)
            {
                if (_eviromentsSelect[i] == environmentSelect)
                {
                    _eviromentsSelect[i] = null;
                    break;
                }
            }
        }

        public void CreatePlayerSelectebleEnviroments()
        {
            for (int i = 0; i < _eviromentsSelect.Count(); i++)
                if (_eviromentsSelect[i] == null)
                    CreateSelectEnviromentServerRpc(i, _turnManager.GetPlayerNumberById(NetworkManager.Singleton.LocalClientId),
                        _enviromentDataConfig.GetRandomIndexSelectSpawnEnviroment);
        }

        [ServerRpc(RequireOwnership = false)]
        private void CreateSelectEnviromentServerRpc(int indexArray, int actualPlayer, int animalIndex)
        {
            GameObject obj = Instantiate(_enviromentDataConfig.GetEnviromentsSelectSpawn(animalIndex),
                _turnManager.GetPlayerInfo(actualPlayer).GetEnvironmentSelectSpawn(indexArray).position,
                _turnManager.GetPlayerInfo(actualPlayer).GetEnvironmentSelectSpawn(indexArray).rotation);
            obj.SetActive(true);
            obj.GetComponent<NetworkObject>().Spawn();
            EnvironmentSelect enviromnentSelect = obj.GetComponent<EnvironmentSelect>();
            enviromnentSelect.Init();
            SyncEnviromentSelectForClientRpc(indexArray, obj.GetComponent<NetworkObject>().NetworkObjectId);
        }


        [ServerRpc(RequireOwnership = false)]
        private void CreateEnveromentServerRpc(int index, int actual, int animalIndex)
        {
            GameObject obj = Instantiate(_enviromentDataConfig.GetEnviroments(animalIndex),
                _turnManager.GetActualPlayerInfo.GetEnvironmentSpawn(index).position,
                _turnManager.GetActualPlayerInfo.GetEnvironmentSpawn(index).rotation);// _prefabEnviroments[randomIndex].transform.rotation);
            obj.SetActive(true);
            obj.GetComponent<NetworkObject>().Spawn();
            GameAnimalsController gameAnimal = obj.GetComponent<GameAnimalsController>();
            gameAnimal.Init();
            SyncAnimalsForClientRpc(index, actual, obj.GetComponent<NetworkObject>().NetworkObjectId);
        }

        [ClientRpc]
        private void SyncAnimalsForClientRpc(int index, int actual, ulong networkIndex)
        {
            NetworkTools.FindNetworkObjectAndMakeAction(networkIndex,
                (networkObject) => _environments[index][actual] = networkObject.gameObject.GetComponent<GameAnimalsController>());
        }

        [ClientRpc]
        private void SyncEnviromentSelectForClientRpc(int index, ulong networkIndex)
        {
            if(NetworkManager.Singleton.LocalClientId == _turnManager.IndexActualPlayer)
                NetworkTools.FindNetworkObjectAndMakeAction(networkIndex,
                    (networkObject) => _eviromentsSelect[index] = networkObject.GetComponent<EnvironmentSelect>());
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

    }
}
