using Harmonies.States;
using Harmonies.Structures;
using UnityEngine;

namespace Harmonies.Selectors
{
    public abstract class ElementSelectorController : MonoBehaviour
    {
        [SerializeField]
        private TurnManager turnManager;

        private Camera _camera;
        private Vector3 _startPosition;
        private bool _isDragging;

        protected bool _unableInteraction;
        public GameCell GameCell { get; set; }
        protected virtual void Start()
        {
            _camera = Camera.main;
            _startPosition = transform.position;
            if(turnManager == null)
                turnManager = FindObjectOfType<TurnManager>();

            turnManager.SubsribeOnStateMachine(OnStatusChange);
        }
        private void Update()
        {
            if (_isDragging && Input.GetMouseButtonUp(0))
            {
                transform.position = _startPosition;
                if (GameCell != null)
                {
                    OnSpawnElementOnCell(GameCell);
                    GameCell = null;
                    //Destroy(gameObject); //object pool in future
                }

            }
        }

        private void OnMouseDrag()
        {
            if (_unableInteraction) return;

            if (Input.GetMouseButton(0))
            {
                Vector3 mousePosition = GetMouseWorldPosition();
                transform.position = new Vector3(mousePosition.x, 1, mousePosition.z);
                _isDragging = true;
            }
        }

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
