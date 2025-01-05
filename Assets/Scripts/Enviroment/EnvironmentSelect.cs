using Harmonies.Enums;
using Harmonies.States;
using System;
using Unity.Netcode;
using UnityEngine;
using Zenject;


namespace Harmonies.Enviroment
{
    public class EnvironmentSelect : NetworkBehaviour
    {
        private EnvironmentController _environmentController;
        private TurnManager _turnManager;
        private bool _unableInteraction;

        [SerializeField]
        private AnimalType _animalType;

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
            if (_turnManager.IndexActualPlayer != NetworkManager.Singleton.LocalClientId)
                return;

            if (_unableInteraction) return;

            if (_environmentController.CanCreate())
            {
                _environmentController.CreatePlayerSelectableEnvironment(_animalType);
                DespawnServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void DespawnServerRpc() => GetComponent<NetworkObject>().Despawn();

        private void OnStatusChange(IState newState) => _unableInteraction = newState is not AnimalsEnvironmentSelectState;
    }
}