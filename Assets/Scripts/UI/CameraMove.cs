using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private Transform[] waypoints;
    [SerializeField]
    private float speed = 2.0f;
    [SerializeField]
    private float rotation = 2.0f; 

    private int currentWaypointIndex = 0;

    private void Update() => MoveToNextWaypoint();

    private void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return; 

        Transform targetWaypoint = waypoints[currentWaypointIndex];


        _camera.transform.position = Vector3.MoveTowards(_camera.transform.position, targetWaypoint.position, speed * Time.deltaTime);

        
        Vector3 direction = targetWaypoint.position - _camera.transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            _camera.transform.rotation = Quaternion.Slerp(_camera.transform.rotation, targetRotation, Time.deltaTime * rotation);
        }

       
        if (Vector3.Distance(_camera.transform.position, targetWaypoint.position) < 0.1f)
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }
}

