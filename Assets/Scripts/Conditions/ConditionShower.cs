using Harmonies.Enums;
using Harmonies.Enviroment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionShower : MonoBehaviour
{
    [SerializeField]
    private Vector3 _offset;
    [SerializeField]
    private Vector3 _offsetRotation;
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
        {
            _showObject = Instantiate(_environmentController.GetPrefabCondition((int)_animalType), Vector3.zero, Quaternion.identity, transform);
            _showObject.transform.localPosition = _offset;
            _showObject.transform.localEulerAngles = _offsetRotation;
        }
    
        _showObject.SetActive(true);
    }

    private void OnMouseExit() => _showObject.SetActive(false);
}
