using Harmonies.States;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Zenject;


namespace Harmonies.Environment
{
    public class EnvironmentSelect : NetworkBehaviour
    {
        private EnvironmentController _environmentController;
        private TurnManager _turnManager;
        private bool _unableInteraction;

        [Inject]
        public void Construct(TurnManager turnManager, EnvironmentController environmentController)
        {
            _turnManager = turnManager;
            _environmentController = environmentController;
        }

        private void Start() //in future Init();
        {
            _turnManager.SubsribeOnStateMachine(OnStatusChange);
        }

        private void OnMouseDown()
        {
            if (_turnManager.IndexActualPlayer != (int)NetworkManager.Singleton.LocalClientId)
                return;

            if (_unableInteraction) return;

            if (_environmentController.CanCreate())
            {
                _environmentController.CreatePlayerSelectableEnvironment();
                DespawnServerRpc();
                //Destroy(gameObject);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void DespawnServerRpc() => GetComponent<NetworkObject>().Despawn();

        private void OnStatusChange(IState newState) => _unableInteraction = newState is not AnimalsEnvironmentSelectState;
    }
}