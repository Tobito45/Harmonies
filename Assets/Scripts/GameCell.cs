using UnityEngine;

public class GameCell : MonoBehaviour
{
    [SerializeField]
    private GameObject _selecter;
    [SerializeField]
    private GameObject[] _objetsToSpawn;

    private BlockController _actualBlock;
    private BoardNode _node;
    public void Init(BoardNode node) => _node = node;

    private void Start()
    {
        _selecter.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_actualBlock == null && other.TryGetComponent(out BlockController block))
        {
            _actualBlock = block;
            _actualBlock.GameCell = this;
            _selecter.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out BlockController block))
        {
            if (block == _actualBlock)
            {
                _selecter.SetActive(false);
                _actualBlock.GameCell = null;
                _actualBlock = null;
            }
        }
    }

    public void SpawnBlock(BlockController block) //in future will change on something like block info
    {
        _selecter.SetActive(false);
        GameObject randomObj = _objetsToSpawn[Random.Range(0, _objetsToSpawn.Length)];
        var obj = Instantiate(randomObj, _selecter.transform.position, randomObj.transform.rotation);
        _selecter.transform.position += new Vector3(0, randomObj.transform.localScale.y * 2, 0);
    }
}
