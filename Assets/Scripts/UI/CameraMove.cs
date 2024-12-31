using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform[] waypoints; 
    public float speed = 2.0f; 
    public float rotation = 2.0f; 

    private int currentWaypointIndex = 0; 

    void Update()
    {
        MoveToNextWaypoint();
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return; 

        Transform targetWaypoint = waypoints[currentWaypointIndex];

       
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        
        Vector3 direction = targetWaypoint.position - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotation);
        }

       
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
 
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}

