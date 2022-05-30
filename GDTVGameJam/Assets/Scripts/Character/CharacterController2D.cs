using System.Collections;
using System.Collections.Generic;
using GlobalTypes;
using UnityEngine;

public class CharacterController2D : BaseController
{
    [Header("Movement Stats")]
    [SerializeField] float airMovementAcceleration = 50f;
    [SerializeField] float jumpSpeed = 30f;
    [SerializeField] float fallMultiplayer =  10f;
    [SerializeField] float coyoteTime = .2f;
    [SerializeField] float jumpBufferTime = .2f;

    [Header("Environment")]
    [SerializeField] public float raycastDistance = 0.2f;
    [SerializeField] public LayerMask layerMask;

    [SerializeField] ParticleSystem movementParticles;
    [SerializeField] ParticleSystem impactParticles;
    ParticleSystem.EmissionModule movementEmission;
    ParticleSystem.MinMaxCurve movementEmissionAmount;

    Animator animator;
    AudioManager audioManager;

    public bool IsJumping;
    public bool IsGrounded; 
    Vector2 _horizontalMoveAmount;
    CapsuleCollider2D _capsuleCollider;
    
    private Vector2 vecGravity;
    private bool wasOnGroundLastFrame = true;
    private bool wasInAirLastFrame = false;
    private Vector2 previousVelocity;
    private float coyoteTimeCounter;
    private float jumpBufferTimeCounter;
    private Transform _tempMovingPlatform;
    private GroundType groundType;

    
    void Start()
    {    
        base.Start();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        animator = GetComponentInChildren<Animator>();
        movementEmission = movementParticles.emission;
        movementEmissionAmount = movementEmission.rateOverTime;
        impactParticles.gameObject.SetActive(false);
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        CheckGrounded();
        if(IsGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        JumpBuffer();
        Vector2 moveAdjustment = Vector2.zero;
        //moving platform adjustment
        if(groundType == GroundType.MovingPlatform)
        {
            //offset the player's movement on the x 
            moveAdjustment = MovingPlatformAdjust();
            //if platform is moving down, offset the player's movement on the y
            if(moveAdjustment.y < 0f)
            {
                moveAdjustment = new Vector2(moveAdjustment.x, moveAdjustment.y * 1.2f);
            }
        }

        if(Mathf.Abs(rigidBody.velocity.x) > maxMoveSpeed)
        {
            rigidBody.velocity =  new Vector2(Mathf.Sign(rigidBody.velocity.x) * maxMoveSpeed, rigidBody.velocity.y);
        }

        animator.SetBool("isWalking", _horizontalMoveAmount.x != 0f && rigidBody.velocity.y == 0f);
        if(wasInAirLastFrame && IsGrounded)
        {
            rigidBody.velocity+= new Vector2(previousVelocity.x * .5f, 0);
        }

        //Reducing sliding after releasing input
        if(_horizontalMoveAmount.x != 0 || moveAdjustment.x != 0)
        {
            Debug.Log(moveAdjustment);
            rigidBody.velocity+=_horizontalMoveAmount+moveAdjustment;
        }else
        {
            if(IsGrounded)
            {
                rigidBody.velocity=new Vector2(0, rigidBody.velocity.y);
            }
        }


        if(IsGrounded && wasInAirLastFrame)
        {
            impactParticles.gameObject.SetActive(true);
            impactParticles.Stop();
            impactParticles.transform.position = movementParticles.transform.position;
            impactParticles.Play();
            audioManager.Play("Landing");
            if(_horizontalMoveAmount.x == 0)
            {
                rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            }
        }

        //Gravity adjustment when jumping and falling
        if(!IsGrounded && rigidBody.velocity.y < 0f)
        {
            if(IsJumping)
            {
                rigidBody.velocity -= vecGravity * fallMultiplayer * Time.deltaTime ;
            }
            else
            {
                rigidBody.velocity -= vecGravity * Time.deltaTime ;
                Debug.Log($"{rigidBody.velocity} {_horizontalMoveAmount}");
                rigidBody.velocity = new Vector2(rigidBody.velocity.x / 2,rigidBody.velocity.y );
            }
        }
        wasOnGroundLastFrame = IsGrounded;
        wasInAirLastFrame = !IsGrounded;
        previousVelocity = rigidBody.velocity;
        _horizontalMoveAmount = Vector2.zero;

    }


    void LateUpdate()
    {
        if(rigidBody.velocity.y < 0.1f && !IsGrounded) {
            animator.SetBool("isFalling", true);
            animator.SetBool("IsJumping", false);
        }
        else if(rigidBody.velocity.y > 0.1f && !IsGrounded)
        {
            animator.SetBool("isFalling", false);
            animator.SetBool("IsJumping", true);
        }
        else
        {
            animator.SetBool("isFalling", false);
            animator.SetBool("IsJumping", false);
        }
        if(rigidBody.velocity.x != 0 && IsGrounded)
        {
            movementEmission.rateOverTime = movementEmissionAmount;
        }
        else
        {
            movementEmission.rateOverTime = 0f;
        }
    }

    void JumpBuffer()
    {
        jumpBufferTimeCounter-=Time.deltaTime;
        if (jumpBufferTimeCounter > 0 && coyoteTimeCounter > 0)
        {
            IsJumping = true;
            rigidBody.velocity += Vector2.up * jumpSpeed;
            coyoteTimeCounter = 0;
            jumpBufferTimeCounter = 0f;
        }
    }

    Vector2 MovingPlatformAdjust()
    {
        if(_tempMovingPlatform && groundType == GroundType.MovingPlatform)
        {
            return _tempMovingPlatform.GetComponent<MovingPlatform>().difference;
        }
        return Vector2.zero;
    }

    public override void Jump(bool jump)
    {
        if(jump)
        {
            jumpBufferTimeCounter = jumpBufferTime;
        }
        
    }

    public override void Move(Vector2 movement)
    {
        if(IsGrounded)
        {
            _horizontalMoveAmount += new Vector2(movement.x * movementAcceleration, 0);
        }  
        else
        {
            _horizontalMoveAmount += new Vector2(movement.x * airMovementAcceleration, 0);
        }
    }


    private void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(_capsuleCollider.bounds.center, _capsuleCollider.size, 0f, Vector2.down, raycastDistance, layerMask);
        //RaycastHit2D hit = Physics2D.CapsuleCast(_capsuleCollider.bounds.center, _capsuleCollider.size, CapsuleDirection2D.Vertical,
        //                                         0f, Vector2.down, raycastDistance, layerMask
        //                                        );
        if(hit.collider)
        {
            IsGrounded = true;
            IsJumping = false;  
            groundType = DetermineGroundType(hit.collider);
        }
        else
        {
            IsGrounded = false;
        }
    }

    private GroundType DetermineGroundType(Collider2D collider)
    {
        if(collider.TryGetComponent<GroundEffector>(out GroundEffector groundEffector))
        {
            if(groundEffector.groundType == GroundType.MovingPlatform || groundEffector.groundType == GroundType.CollapsablePlatform)
            {
                if(!_tempMovingPlatform)
                {
                    _tempMovingPlatform = collider.transform;
                }
            }
            return groundEffector.groundType;
        }
        else
        {
            if(_tempMovingPlatform)
            {
                _tempMovingPlatform = null;
            }
            return GroundType.LevelGeometry;
        }
    }

}
