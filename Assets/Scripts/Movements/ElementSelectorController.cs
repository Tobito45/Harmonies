using Harmonies.Conditions;
using Harmonies.Enums;
using Harmonies.States;
using Harmonies.Structures;
using System;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Harmonies.Selectors
{
    public abstract class ElementSelectorController : NetworkBehaviour
    {
        [SerializeField]
        private ConditionName _conditionName = ConditionName.None;

        protected TurnManager _turnManager;

        private Camera _camera;
        private Vector3 _startPosition;
        private bool _isDragging;

        protected bool _unableInteraction;
        public GameCell GameCell { get; set; }
        protected void InitBase()
        {
            _camera = Camera.main;
            _startPosition = transform.position;
            
            _turnManager.SubsribeOnStateMachine(OnStatusChange);
        }

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
        public virtual bool SelectExceptions(BoardNode<BlockType> node)
        {
            foreach (ConditionName condition in Enum.GetValues(typeof(ConditionName)))
            {
                if (condition == ConditionName.None)
                    continue;

                if ((_conditionName & condition) != 0)
                {
                    if (BaseConditions.GetConditionFunction(condition)(node))
                        return false;
                }
            }

            return true;
        }
        protected abstract void OnStatusChange(IState newState);
    }
}
