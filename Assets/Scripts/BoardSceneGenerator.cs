using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSceneGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab;

    [SerializeField]
    private int _height = 4;
    [SerializeField]
    private int _width = 5;
    private void Start()
    {
        BoardGraph boardGraph = new BoardGraph(_height, _width);
        foreach (var item in boardGraph.GetNodes)
        {
            Debug.Log(item.AllNeighBoursToString());
            Instantiate(_prefab, new Vector3(item.Coordinates.y * 3f, 0, item.Coordinates.x), _prefab.transform.rotation);
        }
    }

    private void Update()
    {
        
    }
}
