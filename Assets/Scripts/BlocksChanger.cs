using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksChanger : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _showObjects;
    [SerializeField]
    private float _timerBetween = 5;
    private float _localTimer = -1;
    private int _actual = -1;

    private void Update()
    {
        if (_localTimer < 0)
            NextItem();
        else
            _localTimer -= Time.deltaTime;
    }

    private void NextItem()
    {
        int prev = _actual;
        _actual++;
        if (_actual >= _showObjects.Length)
            _actual = 0;
        _localTimer = _timerBetween;
        if(prev >= 0)
            _showObjects[prev].SetActive(false);
        _showObjects[_actual].SetActive(true);
    }
}
