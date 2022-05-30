using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : FlyingController
{
    [Header("Ghost Attributes")]
    [SerializeField] public bool hasTimeLimit;
    [SerializeField] float ghostTimer;

    [SerializeField] LayerMask layerMask;
    private Vector2 _moveAmount;
    private float currentGhostTimer;

    public delegate void OnPossession(BaseController character);
    public static event OnPossession onPossession;

    SpriteRenderer sr;

    void Start()
    {
        base.Start();
        currentGhostTimer = ghostTimer;
        sr = GetComponentInChildren<SpriteRenderer>();
    }
    void Update()
    {
        base.Update();
        if(hasTimeLimit)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, (currentGhostTimer/ghostTimer));
            currentGhostTimer -= Time.deltaTime;
        }
        if(currentGhostTimer <= 0 )
        {
            LevelManager.instance.RestartLevel();
            enabled = false;
        }
        if(Mathf.Abs(rigidBody.velocity.x) > maxMoveSpeed)
        {
            rigidBody.velocity =  new Vector2(Mathf.Sign(rigidBody.velocity.x) * maxMoveSpeed, rigidBody.velocity.y);
        }

        CheckForColliders();
    }

    private void CheckForColliders()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, collider2D.bounds.extents.x * 1.5f, Vector2.zero, 0, layerMask);
        if(hit.collider && hit.collider.TryGetComponent<BaseController>(out BaseController character))
        {
            Health health = character.GetComponent<Health>();
            if(health && !health.isDead)
            {
                Debug.Log("possess");
                onPossession(character);
                GameObject.Destroy(gameObject);
            }
        }
    }
}
