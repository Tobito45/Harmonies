using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnvironmentController : MonoBehaviour
{
    [SerializeField]
    private Transform[] _spawnPoints;

    [SerializeField]
    private GameObject _prefabEnviroment;
    private GameObject[] _environments = new GameObject[4];

    public void CreatePlayerSelectableEnvironment()
    {
        for (int i = 0; i < _environments.Length; i++)
        {
            if (_environments[i] == null)
            {
                GameObject obj = Instantiate(_prefabEnviroment, _spawnPoints[i].position, _prefabEnviroment.transform.rotation);
                obj.SetActive(true);
                obj.GetComponent<GameAnimal>().Init(this);
                _environments[i] = obj;
                return;
            }
        }
    }

    public void DeletePlayerSelectableEnviroment(GameAnimal animal)
    {
        for (int i = 0; i < _environments.Length; i++)
        {
            if (_environments[i] == animal)
                _environments = null;

            Destroy(animal.gameObject);
        }

    }

    public bool CanCreate()
    {
        for (int i = 0; i < _environments.Length; i++)
            if (_environments[i] == null) return true;
       
        return false;
    }

}
