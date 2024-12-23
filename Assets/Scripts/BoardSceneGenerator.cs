using Harmonies.Structures;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BoardSceneGenerator : NetworkBehaviour
{
    [SerializeField]
    private GameObject _prefab;

    [SerializeField]
    private Transform _offset;

    [SerializeField]
    private int _height = 4;
    [SerializeField]
    private int _width = 5;
    public BoardGraph BoardGraph { get; private set; }
    public void Init()
    {
        if (!IsOwner) return;

        CreateBoardGraphClientRpc(_height, _width);
        
        foreach ((BoardNode item, int index) in BoardGraph.GetNodesWithIndex())
        {
            var obj = Instantiate(_prefab, new Vector3(item.Coordinates.y * 3f, 0, item.Coordinates.x) + _offset.position, _prefab.transform.rotation);
            obj.GetComponent<NetworkObject>().Spawn();
            GameCell gameCell = obj.GetComponent<GameCell>();
            item.GameCell = gameCell;
            gameCell.Init(index);
        }
        //InitAllBoardClientRpc();
    }

    [ClientRpc]
    private void CreateBoardGraphClientRpc(int _height, int _width) {
        BoardGraph boardGraph = new BoardGraph(_height, _width);
        BoardGraph = boardGraph;
        Debug.Log(boardGraph);
    }

}
