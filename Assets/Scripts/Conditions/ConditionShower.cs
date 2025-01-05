using Harmonies.Enums;
using Harmonies.Enviroment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionShower : MonoBehaviour
{
    [SerializeField]
    private Vector3 _offset;
    private EnvironmentController _environmentController;
    private AnimalType _animalType;
    private GameObject _showObject;

    public void Init(EnvironmentController environmentController, AnimalType animalType)
    {
        _environmentController = environmentController; 
        _animalType = animalType;
    }

    private void OnMouseEnter()
    {
        if(_showObject == null)
            _showObject = Instantiate(_environmentController.GetPrefabCondition((int)_animalType), _offset, Quaternion.identity);
    
        _showObject.SetActive(true);
    }

    private void OnMouseExit() => _showObject.SetActive(false);
}
