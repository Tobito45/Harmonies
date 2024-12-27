using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Netcode;
using Unity.VisualScripting;
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
            for (int i = 0; i < _environments.Length; i++)
            {
                if (_environments[i][_turnManager.IndexActualPlayer] == null)
                {
                    CreateEnveromentServerRpc(i, _turnManager.IndexActualPlayer);
                    StartCoroutine(WaitForServer(i, _turnManager.IndexActualPlayer));
                    return;
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void CreateEnveromentServerRpc(int index, int actual)
        {
            GameObject obj = Instantiate(_prefabEnviroment, _turnManager.GetActualPlayerInfo.GetEnvironmentSpawn(index).position, _prefabEnviroment.transform.rotation);
            obj.SetActive(true);
            obj.GetComponent<NetworkObject>().Spawn();
            GameAnimal gameAnimal = obj.GetComponent<GameAnimal>();
            gameAnimal.Init();
            SyncForClientRpc(index, actual, obj.GetComponent<NetworkObject>().NetworkObjectId);

        }

        [ClientRpc]
        private void SyncForClientRpc(int index, int actual, ulong networkIndex)
        {
            if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkIndex, out var networkObject))
                _environments[index][actual] = networkObject.gameObject.GetComponent<GameAnimal>();
            else
                Debug.LogError("ERROR");
        }

        public IEnumerator WaitForServer(int index, int actual)
        {
            while(_environments[index][actual] == null)
                yield return null;  

            _turnManager.WasSelectedOrSkipedAnimalsEnviroment();
        }


        public void DeletePlayerSelectableEnviroment(GameAnimal animal)
        {
            for (int i = 0; i < _environments.Length; i++)
            {
                if (_environments[i][_turnManager.IndexActualPlayer] == animal)
                    _environments[i][_turnManager.IndexActualPlayer] = null;

                DestroyServerRpc(animal.GetComponent<NetworkObject>().NetworkObjectId);
                //Destroy(animal.gameObject);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void DestroyServerRpc(ulong networkIndex)
        {
            if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkIndex, out var networkObject))
                networkObject.Despawn();
            else
                Debug.LogError("ERROR");
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
