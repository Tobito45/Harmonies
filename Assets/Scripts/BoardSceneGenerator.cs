using Harmonies.Structures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSceneGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject _prefab;

    [SerializeField]
    private Transform _offset;

    [SerializeField]
    private int _height = 4;
    [SerializeField]
    private int _width = 5;
    private void Start()
    {
        BoardGraph boardGraph = new BoardGraph(_height, _width);
        foreach (var item in boardGraph.GetNodes)
        {
            var obj = Instantiate(_prefab, new Vector3(item.Coordinates.y * 3f, 0, item.Coordinates.x) + _offset.position, _prefab.transform.rotation);
            GameCell gameCell = obj.GetComponent<GameCell>();
            item.GameCell = gameCell;
            gameCell.Init(item);
        }
    }

}
