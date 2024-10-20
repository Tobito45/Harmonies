using Harmonies.Selectors;
using Harmonies.Structures;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameCell : MonoBehaviour
{
    [SerializeField]
    private GameObject _selecter;
    [SerializeField]
    private GameObject[] _animalsToSpawn;

    private ElementSelectorController _actualBlock;
    private BoardNode _node;
    private bool _isAnimalOn = false;
    public void Init(BoardNode node) => _node = node;

    private void Start() => _selecter.SetActive(false);
    private void OnTriggerEnter(Collider other)
    {
        if (_isAnimalOn)
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
        if (_isAnimalOn)
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
        var obj = Instantiate(block.Prefab, _selecter.transform.position, block.Prefab.transform.rotation);
        _selecter.transform.position += new Vector3(0, block.Prefab.transform.localScale.y * 2, 0);
    }

    public void SpawnAnimal(ElementSelectorController block) //in future will change on something like block info
    {
        _selecter.SetActive(false);
        _isAnimalOn = true;
        GameObject randomObj = _animalsToSpawn[Random.Range(0, _animalsToSpawn.Length)];
        var obj = Instantiate(randomObj, _selecter.transform.position, randomObj.transform.rotation);
    }
}
