using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingController : BaseController
{
    Vector2 _moveAmount;

    public void Update()
    {
        if(Mathf.Abs(rigidBody.velocity.x) > maxMoveSpeed)
        {
            rigidBody.velocity =  new Vector2(Mathf.Sign(rigidBody.velocity.x) * maxMoveSpeed, rigidBody.velocity.y);
        }
        rigidBody.AddForce(_moveAmount, ForceMode2D.Impulse);   
        _moveAmount = Vector2.zero;
    }

    public override void Move(Vector2 movement)
    {
        _moveAmount += movement * movementAcceleration;
    }
}
