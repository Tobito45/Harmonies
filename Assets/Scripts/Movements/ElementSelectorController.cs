using Harmonies.States;
using Harmonies.Structures;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Harmonies.Selectors
{
    public abstract class ElementSelectorController : NetworkBehaviour
    {
        [SerializeField]
        private TurnManager _turnManager;

        private Camera _camera;
        private Vector3 _startPosition;
        private bool _isDragging;

        protected bool _unableInteraction;
        public GameCell GameCell { get; set; }
        protected virtual void Start()
        {
            _camera = Camera.main;
            _startPosition = transform.position;
            if(_turnManager == null)
                _turnManager = FindObjectOfType<TurnManager>();
            
            _turnManager.SubsribeOnStateMachine(OnStatusChange);
        }

        [Inject]
        public void Construct(TurnManager turnManager) => _turnManager = turnManager;

        private void Update()
        {
            if (_isDragging && Input.GetMouseButtonUp(0))
            {
                MoveObjectServerRpc(_startPosition);

                if (GameCell != null)
                {
                    OnSpawnElementOnCell(GameCell);
                    GameCell = null;
                }

            }
        }

        private void OnMouseDrag()
        {
            if (_unableInteraction) return;

            if (Input.GetMouseButton(0))
            {

                if (_camera == null)
                    _camera = Camera.main;

                Vector3 mousePosition = GetMouseWorldPosition();
                Vector3 newPosition = new Vector3(mousePosition.x, 1, mousePosition.z);
                MoveObjectServerRpc(newPosition);
                _isDragging = true;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void MoveObjectServerRpc(Vector3 newPosition) => transform.position = newPosition;


        private Vector3 GetMouseWorldPosition()
        {
            Vector3 mouseScreenPosition = Input.mousePosition;
            mouseScreenPosition.z = _camera.WorldToScreenPoint(transform.position).z;
            return _camera.ScreenToWorldPoint(mouseScreenPosition);
        }

        protected abstract void OnSpawnElementOnCell(GameCell gameCell);
        public abstract bool SelectExceptions(BoardNode node);
        protected abstract void OnStatusChange(IState newState);
    }
}
