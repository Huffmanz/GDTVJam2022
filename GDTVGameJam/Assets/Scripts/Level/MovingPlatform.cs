using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 5f;
    public Vector2 difference;

    private Vector3 _lastPosition;
    private Vector3 _currentWaypoint;
    private int _wayPointCounter;

    void Start()
    {
        _wayPointCounter = 0;
        _currentWaypoint = waypoints[_wayPointCounter].position;
    }

    // Update is called once per frame
    void Update()
    {
        if(waypoints == null || waypoints.Length == 0) return;
        _lastPosition = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, _currentWaypoint, moveSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position, _currentWaypoint) < 0.1f)
        {
            _wayPointCounter++;
            if(_wayPointCounter >= waypoints.Length)
            {
                _wayPointCounter = 0;
            }
            _currentWaypoint = waypoints[_wayPointCounter].position;
        }
        difference = transform.position - _lastPosition;
        
    }
}
