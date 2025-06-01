using Harmonies.Selectors;
using Harmonies.States;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Harmonies.Blocks
{
    public class BlocksSelect : NetworkBehaviour
    {
        [field: SerializeField]
        public bool IsControlled { get; set; } = false;

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
            if (!IsOwner) return;

            for (int i = 0; i < _spawnPoints.Length; i++)
            {
                if (_spawnedObjects[i] == null)
                {
                    GameObject obj = _spawnBlocksController.GetRandomSpawnBlock;
                    var created = Instantiate(obj, _spawnPoints[i].transform.position, _spawnPoints[i].transform.rotation);
                    created.SetActive(true);
                    created.GetComponent<NetworkObject>().Spawn();
                    SyncForClientRpc(i, created.GetComponent<NetworkObject>().NetworkObjectId);
                }
            }
        }

        [ClientRpc]
        private void SyncForClientRpc(int index, ulong networkIndex)
        {
            NetworkTools.FindNetworkObjectAndMakeAction(networkIndex,
               (networkObject) => {
                   var select = networkObject.GetComponent<BlockSelectorController>();
                   select.Init();
                   _spawnedObjects[index] = select;
               });
        }


        private void OnMouseDown()
        {
            if (!IsControlled) return;

            if (_turnManager.IndexActualPlayer != NetworkManager.Singleton.LocalClientId)
                return;

            if (!_isActive)
                return;

            ActiveBlocksSelectorServerRpc(true);
            _spawnBlocksController.SetAlreadySpawnedBlocks(_spawnedObjects);
            _turnManager.WasSelectedBlocksSelector();
        }

        [ServerRpc(RequireOwnership = false)]
        public void ActiveBlocksSelectorServerRpc(bool enabled) => ChangeColorClientRpc(enabled);

        [ClientRpc]
        public void ChangeColorClientRpc(bool enabled)
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
                ActiveBlocksSelectorServerRpc(false);
        }
    }
}
