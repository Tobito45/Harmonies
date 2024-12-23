using Harmonies.Selectors;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Events;
using Zenject;


namespace Harmonies.Environment
{
    public class GameAnimal : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer[] _animalsGameObjects;
        private Material[] _basicMaterials;

        [SerializeField] // in future in Resoursec
        private Material _materialUnIterractble;
        private EnvironmentController _environmentController;
        private TurnManager _turnManager;
        private int _index;

        public void Init(EnvironmentController environmentController, TurnManager turnManager)
        {
            _environmentController = environmentController;
            _turnManager = turnManager;

            _basicMaterials = new Material[_animalsGameObjects.Length];
            _index = -1;
            for (int i = 0; i < _animalsGameObjects.Length; i++)
            {
                _animalsGameObjects[i].gameObject.SetActive(true);
                _animalsGameObjects[i].GetComponent<SphereCollider>().enabled = false;

                if (_basicMaterials[i] == null)
                    _basicMaterials[i] = _animalsGameObjects[i].material;

                _animalsGameObjects[i].material = _materialUnIterractble;
            }
            ActiveNextAnimal();
        }


        public void AnimalWasSelected()
        {
            _animalsGameObjects[_index].material = _basicMaterials[_index];
            _animalsGameObjects[_index].gameObject.SetActive(false);
            //_animalsGameObjects[_index].gameObject.GetComponent<NetworkObject>().Despawn();

            ActiveNextAnimal();
            
            if (!_turnManager.IsAnyEnviroment())
                _turnManager.WasAnimalsSkiped();

        }

        public void ActiveNextAnimal()
        {
            _index++;
            if (_index >= _animalsGameObjects.Length)
            {
                _environmentController.DeletePlayerSelectableEnviroment(this);
                return;
            }

            _animalsGameObjects[_index].material = _basicMaterials[_index];
            _animalsGameObjects[_index].GetComponent<SphereCollider>().enabled = true;
        }
    }
}
