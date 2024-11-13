using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private Transform[] _playersPosition;
    [SerializeField]
    private float _smoothSpeed = 5f;

    private TurnManager _turnManager;
    private Vector3 _targetPosition;
    private bool _isMoving;
    [Inject]
    public void Construct(TurnManager turnManager)
    {
        _turnManager = turnManager;
        _turnManager.OnRoundEnded += OnRoundEnded;
    }

    private void OnRoundEnded(int previous, int actual)
    {
        _targetPosition = _playersPosition[actual].position;
        _isMoving = true;
    }
    private void Update()
    {
        if (_isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, _smoothSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _targetPosition) < 0.1f)
                _isMoving = false; 
        }
    }
}
