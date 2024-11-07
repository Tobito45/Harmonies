using Harmonies.Selectors;
using Harmonies.States;
using UnityEngine;
using Zenject;

namespace Harmonies.Blocks
{
    public class BlocksSelect : MonoBehaviour
    {
        [SerializeField]
        private MeshRenderer _meshRenderer;
        [SerializeField]
        private Material _selectMaterial;
        private Material _baseMaterial;


        [SerializeField]
        private Collider _collider;

        [Header("Spawn objects")]
        [SerializeField]
        private Transform[] _spawnPoints;

        [SerializeField]
        private GameObject[] _spawnObjectsPrefabs;

        private BlockSelectorController[] _spawnedObjects;
        private SpawnBlocksController _spawnBlocksController;
        private TurnManager _turnManager;
        private bool _isActive = true;

        [Inject]
        public void Construct(TurnManager turnManager, SpawnBlocksController spawnBlocksController)
        {
            _turnManager = turnManager;
            _spawnBlocksController = spawnBlocksController;
        }

        private void Start()
        {
            _spawnedObjects = new BlockSelectorController[_spawnPoints.Length];
            _meshRenderer = GetComponent<MeshRenderer>();
            _baseMaterial = _meshRenderer.material;

            if (_spawnBlocksController == null)
                _spawnBlocksController = FindObjectOfType<SpawnBlocksController>(); // to zenject

            _turnManager.SubsribeOnStateMachine(OnStatusChange);
        }

        public void ActualizeSpawndeObjects()
        {
            for (int i = 0; i < _spawnPoints.Length; i++)
            {
                if (_spawnedObjects[i] == null)
                {
                    GameObject obj = _spawnObjectsPrefabs[Random.Range(0, _spawnObjectsPrefabs.Length)];
                    var created = Instantiate(obj, _spawnPoints[i].transform.position, _spawnPoints[i].transform.rotation);
                    created.SetActive(true);
                    var select = created.GetComponent<BlockSelectorController>();
                    select.Init(_spawnBlocksController);
                    _spawnedObjects[i] = select;
                }
            }
        }

        private void OnMouseDown()
        {
            if (!_isActive)
                return;

            ActiveBlocksSelector(true);
            _spawnBlocksController.SetAlreadySpawnedBlocks(_spawnedObjects);
            _turnManager.WasSelectedBlocksSelector();
        }

        public void ActiveBlocksSelector(bool enabled)
        {
            _collider.enabled = !enabled;
            if (enabled)
                _meshRenderer.material = _selectMaterial;
            else
                _meshRenderer.material = _baseMaterial;

        }

        private void OnStatusChange(IState newState)
        {
            _isActive = newState is BlockSelectState;

            if (newState is BlockSelectState)
                ActualizeSpawndeObjects();
            else if (newState is AnimalsEnvironmentSelectState) //not is BlocksPlaceSelectState dint work??
                ActiveBlocksSelector(false);
        }
    }
}
