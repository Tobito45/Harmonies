using Harmonies.Enums;
using Harmonies.InitObjets;
using Harmonies.Score;
using Harmonies.States;
using System;
using Unity.Netcode;
using UnityEngine;
using Zenject;


namespace Harmonies.Enviroment
{
    public class EnvironmentSelect : NetworkBehaviour
    {
        [field: SerializeField]
        public bool IsControlled { get; set; } = false;

        private EnvironmentController _environmentController;
        private TurnManager _turnManager;
        private bool _unableInteraction;

        [SerializeField]
        private AnimalType _animalType;

        public void Init(TurnManager turnManager, EnvironmentController environmentController)
        {
            _turnManager = turnManager;
            _environmentController = environmentController;
            _turnManager.SubsribeOnStateMachine(OnStatusChange);
            GetComponent<ConditionShower>().Init(_animalType);
        }

        public void Init() => InitClientRpc();

        [ClientRpc]
        private void InitClientRpc()
        {
            if (InitObjectsFactory.InitObjects.TryGetValue(GetType(), out Action<object> method))
                method(this);
        }

        //[Inject]
        //public void Construct(TurnManager turnManager, EnvironmentController environmentController)
        //{
        //    _turnManager = turnManager;
        //    _environmentController = environmentController;
        //}

        private void OnMouseDown()
        {
            if (!IsControlled) return;

            if (_turnManager.IndexActualPlayer != NetworkManager.Singleton.LocalClientId)
                return;
            Debug.Log(_unableInteraction);

            if (_unableInteraction) return;

            Debug.Log(_unableInteraction);
            if (_environmentController.CanCreate())
            {
                _environmentController.CreatePlayerSelectedEnvironment(_animalType, this);
                DespawnServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void DespawnServerRpc() => GetComponent<NetworkObject>().Despawn();

        private void OnStatusChange(IState newState)
        {
            Debug.Log(newState + " " + (newState is not AnimalsEnvironmentSelectState));
            _unableInteraction = newState is not AnimalsEnvironmentSelectState;
        }
    }
}