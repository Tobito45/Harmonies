using Harmonies.Enums;
using Harmonies.Score;
using Harmonies.Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Zenject;

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

    private ScoreController _scoreController;
    public BoardGraph<BlockType> BoardGraph { get; private set; }

    [Inject]
    private void Construct(ScoreController scoreController) => _scoreController = scoreController;

    public void Init(int i)
    {
        if (!IsOwner) return;

        CreateBoardGraphClientRpc(_height, _width);
        
        foreach ((BoardNode<BlockType> item, int index) in BoardGraph.GetNodesWithIndex())
        {
            var obj = Instantiate(_prefab, new Vector3(item.Coordinates.y * 3f, 0, item.Coordinates.x) + _offset.position, _prefab.transform.rotation);
            obj.GetComponent<NetworkObject>().Spawn();
            SyncForClientRpc(obj.GetComponent<NetworkObject>().NetworkObjectId, index, i);
        }
    }

    [ClientRpc]
    private void SyncForClientRpc(ulong networkIndex, int index, int i)
    {
        NetworkTools.FindNetworkObjectAndMakeAction(networkIndex,
           (networkObject) => {
               GameCell gameCell = networkObject.GetComponent<GameCell>();
               BoardGraph.GetNodeByIndex(index).GameCell = gameCell;
               gameCell.Init(index, i);
           });
    }

    [ClientRpc]
    private void CreateBoardGraphClientRpc(int _height, int _width) {
        BoardGraph<BlockType> boardGraph = new BoardGraph<BlockType>(_height, _width);
        BoardGraph = boardGraph;
        _scoreController.CountFreeCells = boardGraph.Count;
    }

}
