using Harmonies.InitObjets;
using Harmonies.Selectors;
using Harmonies.Structures;
using System;
using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameCell : NetworkBehaviour
{
    [SerializeField]
    private GameObject _selecter;
    [SerializeField]
    private GameObject[] _animalsToSpawn;
    [SerializeField]
    private GameObject[] _blocksToSpawn;

    private ElementSelectorController _actualBlock;
    private TurnManager _turnManager;
    private BoardNode _node;
    private bool _isAnimalOn = false;
    public void Init(BoardNode node, TurnManager turnManager)
    {
        _turnManager = turnManager;
        _node = node;
    }

    public void Init(int index, int i) =>
        StartCoroutine(InitObjectsFactory.WaitForCallbackWithPredicate(typeof(GameCell),
            (this, index, i), () => InitClientRpc(index, i)));

    [ClientRpc]
    private void InitClientRpc(int index, int i)
    {
        if (InitObjectsFactory.InitObject.TryGetValue(GetType(), out Action<object> method))
            method((this, index, i));
    }

    private void Start() => _selecter.SetActive(false);
    private void OnTriggerEnter(Collider other)
    {
        if (_isAnimalOn || _turnManager.IndexActualPlayer != (int)NetworkManager.Singleton.LocalClientId)
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
        if (_isAnimalOn || _turnManager.IndexActualPlayer != (int)NetworkManager.Singleton.LocalClientId)
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
        Vector3 sync = _selecter.transform.position + new Vector3(0, block.Prefab.transform.localScale.y * 2, 0);
        CreateBlockServerRpc(block.Index);
        
        if(_selecter.transform.position != sync)
            _selecter.transform.position += new Vector3(0, block.Prefab.transform.localScale.y * 2, 0);
        
        block.DisableServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void CreateBlockServerRpc(int index)
    {
        GameObject block = _blocksToSpawn[index];
        var obj = Instantiate(block, _selecter.transform.position, block.transform.rotation);
        _selecter.transform.position += new Vector3(0, block.transform.localScale.y * 2, 0);
        obj.GetComponent<NetworkObject>().Spawn();
    }

    public void SpawnAnimal(ElementSelectorController block) //in future will change on something like block info
    {
        _selecter.SetActive(false);
        _isAnimalOn = true;

        CreateAnimalServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void CreateAnimalServerRpc()
    {
        GameObject randomObj = _animalsToSpawn[UnityEngine.Random.Range(0, _animalsToSpawn.Length)];
        var obj = Instantiate(randomObj, _selecter.transform.position, randomObj.transform.rotation);
        obj.GetComponent<NetworkObject>().Spawn();
    }

}
