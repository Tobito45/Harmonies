using Harmonies.Selectors;
using Harmonies.Structures;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class GameCell : NetworkBehaviour
{
    [SerializeField]
    private GameObject _selecter;
    [SerializeField]
    private GameObject[] _animalsToSpawn;
    [SerializeField]
    private GameObject[] _blocksToSpawn;

    private ElementSelectorController _actualBlock;
    private BoardNode _node;
    private bool _isAnimalOn = false;
    public void Init(BoardNode node) => _node = node;
    public void Init(int index) => StartCoroutine(WaitForBoardGraphInitialization(index));// InitClientRpc(index);

    [ClientRpc]
    private void InitClientRpc(int index)
    {
        Debug.Log(FindObjectOfType<BoardSceneGenerator>().BoardGraph);
        _node = FindObjectOfType<BoardSceneGenerator>().BoardGraph.GetNodeByIndex(index);
    }

    private IEnumerator WaitForBoardGraphInitialization(int index)
    {
        float timer = 0f;
        while (FindObjectOfType<BoardSceneGenerator>().BoardGraph == null)
        {
            timer += 0.1f;
            yield return null;
            if (timer > 5000)
                break;
        }

        InitClientRpc(index);
    }

    private void Start() => _selecter.SetActive(false);
    private void OnTriggerEnter(Collider other)
    {
        if (_isAnimalOn || FindObjectOfType<TurnManager>().IndexActualPlayer != (int)NetworkManager.Singleton.LocalClientId)
            return;

        if (_actualBlock == null && other.TryGetComponent(out ElementSelectorController block))
        {
            if(block.SelectExceptions(_node))  
                return;

            _actualBlock = block;
            _actualBlock.GameCell = this;
            _selecter.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isAnimalOn || FindObjectOfType<TurnManager>().IndexActualPlayer != (int)NetworkManager.Singleton.LocalClientId)
            return;

        if (other.TryGetComponent(out ElementSelectorController block))
        {
            if (block == _actualBlock)
            {
                _selecter.SetActive(false);
                _actualBlock.GameCell = null;
                _actualBlock = null;
            }
        }
    }

    public void SpawnBlock(GameBlock block) //in future will change on something like block info
    {
        _selecter.SetActive(false);
        _node.AddNewIndex(block.Index);
        CreateBlockServerRpc(block.Index);
        _selecter.transform.position += new Vector3(0, block.Prefab.transform.localScale.y * 2, 0);
        block.DisableServerRpc();
        //block.GetComponent<NetworkObject>().Despawn();
        //Destroy(block.gameObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void CreateBlockServerRpc(int index)
    {
        GameObject block = _blocksToSpawn[index];
        var obj = Instantiate(block, _selecter.transform.position, block.transform.rotation);
        obj.GetComponent<NetworkObject>().Spawn();
    }

    public void SpawnAnimal(ElementSelectorController block) //in future will change on something like block info
    {
        if (!IsOwner)
            return;
        
        _selecter.SetActive(false);
        _isAnimalOn = true;
        GameObject randomObj = _animalsToSpawn[UnityEngine.Random.Range(0, _animalsToSpawn.Length)];
        var obj = Instantiate(randomObj, _selecter.transform.position, randomObj.transform.rotation);
        obj.GetComponent<NetworkObject>().Spawn();
    }
}
