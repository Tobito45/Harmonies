using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    [SerializeField]
    private Vector3 _offset;
    [SerializeField]
    private Camera _camera;

    private Vector3 _startPosition;
    private bool _isDragging;

    public GameCell GameCell { get; set; }
    private void Start()
    {
        _startPosition = transform.position;
    }
    private void Update()
    {
        if (_isDragging && Input.GetMouseButtonUp(0))
        {
            transform.position = _startPosition;
            if(GameCell != null)
            {
                GameCell.SpawnBlock(this);
                //Destroy(gameObject); //object pool in future
            }

        }
    }

    private void OnMouseDrag()
    {
        if(Input.GetMouseButton(0))
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            transform.position = new Vector3(mousePosition.x, 1, mousePosition.z) + _offset;
            _isDragging = true;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = _camera.WorldToScreenPoint(transform.position).z;
        return  _camera.ScreenToWorldPoint(mouseScreenPosition);
    }
}
