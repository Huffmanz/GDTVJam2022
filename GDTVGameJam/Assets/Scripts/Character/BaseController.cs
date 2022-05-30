using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    [Header("Movement Stats")]
    [SerializeField] public float movementAcceleration = 50;
    [SerializeField] public float maxMoveSpeed = 12f;
    public Rigidbody2D rigidBody { get; private set;}
    public Collider2D collider2D { get; private set;}

    public bool CanFly;
    public PlayerController player;
    
    public void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
    }

    public virtual void Move(Vector2 movement)
    {
        
    }

    public virtual void Jump(bool jump)
    {

    }
}
