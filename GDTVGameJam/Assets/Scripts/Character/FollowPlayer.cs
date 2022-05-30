using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class FollowPlayer : MonoBehaviour
{
    public Transform subject;
    float startY;

    
    void Start()
    {
        startY = transform.position.y;
    }
    // Update is called once per frame
    void Update () {
        Vector3 newPos = new Vector3(subject.transform.position.x,subject.transform.position.y,0);
        transform.position = newPos + new Vector3(0, 0, -5);
    }
}
