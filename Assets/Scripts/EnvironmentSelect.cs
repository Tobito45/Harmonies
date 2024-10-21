using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSelect : MonoBehaviour
{
    [SerializeField]
    private EnvironmentController _environmentController;

    private void Start() //in future Init();
    {
        if (_environmentController == null)
            _environmentController = FindObjectOfType<EnvironmentController>();
    }

    private void OnMouseDown()
    {
        if (_environmentController.CanCreate())
        {
            _environmentController.CreatePlayerSelectableEnvironment();
            Destroy(gameObject);
        }
    }
}
