using Harmonies.InitObjets;
using Harmonies.Score.AnimalCard;
using Harmonies.Selectors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using Zenject;


namespace Harmonies.Enviroment
{
    public class GameAnimalsController : NetworkBehaviour
    {
        [SerializeField]
        private Renderer[] _animalsMeshRenders;
        [SerializeField]
        private AnimalSelectorController[] _animalsObjects;
        private Material[] _basicMaterials;

        [SerializeField] // in future in Resoursec
        private Material _materialUnIterractble;

        [field: SerializeField]
        public GameAnimal GameAnimal { get; private set; }

        private EnvironmentController _environmentController;
        private TurnManager _turnManager;
        private int _index;

        public int Index => _index;
        public void Init(EnvironmentController environmentController, TurnManager turnManager, AnimalsCardsUI animalsCardsUI)
        {
            _environmentController = environmentController;
            _turnManager = turnManager;

            if (GameAnimal == null)
                GameAnimal = GetComponent<GameAnimal>();

            if (_turnManager.IndexActualPlayer == NetworkManager.Singleton.LocalClientId)
                animalsCardsUI.SummonNewAnimalCard(GameAnimal);

            GetComponent<ConditionShower>().Init(GameAnimal.Index);
        }

        public void Init() => InitClientRpc();

        [ClientRpc]
        private void InitClientRpc()
        {
            if (InitObjectsFactory.InitObjects.TryGetValue(GetType(), out Action<object> method))
                method(this);

            _basicMaterials = new Material[_animalsMeshRenders.Length];
            _index = -1;
            for (int i = 0; i < _animalsMeshRenders.Length; i++)
            {
                if (InitObjectsFactory.InitObjects.TryGetValue(typeof(AnimalSelectorController), out Action<object> methodAnimal))
                    methodAnimal(_animalsObjects[i].GetComponent<AnimalSelectorController>());

                _animalsObjects[i].gameObject.SetActive(true);
                _animalsObjects[i].GetComponent<SphereCollider>().enabled = false;

                if (_turnManager.IndexActualPlayer == NetworkManager.Singleton.LocalClientId)
                {
                    if (_basicMaterials[i] == null)
                        _basicMaterials[i] = _animalsMeshRenders[i].material;

                    _animalsMeshRenders[i].material = _materialUnIterractble;
                }
            }

            if (_turnManager.IndexActualPlayer == NetworkManager.Singleton.LocalClientId)
                ActiveNextAnimal();
        }

        public void SetUnableInteracte()
        {
            foreach(AnimalSelectorController animalSelectorController in _animalsObjects)
                animalSelectorController.SetNotInteractable();
        }
        public void AnimalWasSelected()
        {
            _animalsMeshRenders[_index].material = _basicMaterials[_index];
            _animalsObjects[_index].gameObject.SetActive(false);
            DisableServerRpc(_index);

            ActiveNextAnimal();
            
            if (!_turnManager.IsAnyEnviroment())
                _turnManager.WasAnimalsSkiped();

        }

        [ServerRpc(RequireOwnership = false)]
        private void DisableServerRpc(int index)
        {
            _animalsObjects[index].gameObject.SetActive(false);
            DisableClientRpc(index);
        }

        [ClientRpc]
        private void DisableClientRpc(int index) => _animalsObjects[index].gameObject.SetActive(false);

        public void ActiveNextAnimal()
        {
            _index++;
            if (_index >= _animalsMeshRenders.Length)
            {
                _environmentController.DeletePlayerSelectableEnviroment(this);
                return;
            }

            _animalsMeshRenders[_index].material = _basicMaterials[_index];
            _animalsObjects[_index].GetComponent<SphereCollider>().enabled = true;
        }
    }
}
