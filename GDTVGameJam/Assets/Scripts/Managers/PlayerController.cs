using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GhostController ghost;
    [SerializeField] CinemachineVirtualCamera virtualCam;
    [SerializeField] BaseController startingCharacter;
    

    public bool GhostMode;

    FollowPlayer followPlayer;
    BaseController _characterController;

    Vector2 _input;
    public Vector2 Input { get {return _input;} }
    bool _startJump;
    public bool StartJump { get {return _startJump;} }
    bool _releaseJump;
    bool _holdJump;

    public Vector2 _moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        if(startingCharacter.TryGetComponent<GhostController>(out GhostController ghost))
        {
            GhostMode = true;
        }
        _characterController = startingCharacter;
        _characterController.player = this;
        GhostController.onPossession += Possess;
        followPlayer = Camera.main.GetComponent<FollowPlayer>();
        followPlayer.subject = startingCharacter.transform;
    }

    void OnDisable()
    {
        GhostController.onPossession -= Possess;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessHorizontalMovement();
        ProcessVertialMovement();
        Jump();
        _characterController.Move(_moveDirection * Time.deltaTime );
        
    }

    void ProcessHorizontalMovement()
    {
       _moveDirection.x = _input.x;
        if(_moveDirection.x < 0)
        {
            _characterController.gameObject.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if(_moveDirection.x > 0)
        {

            _characterController.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        }
    }

    void ProcessVertialMovement()
    {
        _moveDirection.y = _input.y;
    }

    void Jump()
    {
        if(_startJump)
        {
            _characterController.Jump(_startJump);
            _startJump = false;
        }
        else
        {
            _characterController.Jump(_startJump);
        }
    }

    public void EnterGhostMode()
    {
        GhostMode = true;
        SpawnGhost();
    }

    void SpawnGhost()
    {
        BaseController newCharacter = GameObject.Instantiate(ghost.gameObject).GetComponent<BaseController>();
        newCharacter.player = this;
        if(_characterController)
        {
            newCharacter.gameObject.transform.position = _characterController.gameObject.transform.position;
            Destroy(_characterController.gameObject);
        }
        _characterController = newCharacter;
        followPlayer.subject = _characterController.gameObject.transform;

    }

    void Possess(BaseController character)
    {
        if(_characterController)
        {
            Destroy(_characterController.gameObject);
        }
        _characterController = character;
        _characterController.player = this;
        followPlayer.subject = _characterController.gameObject.transform;
        GhostMode = false;
        _moveDirection = Vector2.zero;
        MovingPlatform movingPlatform = _characterController.GetComponentInChildren<MovingPlatform>();
        if(movingPlatform)
        {
            movingPlatform.enabled = false;
        }
    }

    #region Input Actions

    public void OnMovement(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            _startJump = true;
            _releaseJump = false;
            _holdJump = true;
        }
        else if(context.canceled)
        {
            _releaseJump = true;
            _startJump = false;
            _holdJump = false;  
        }
    }

    public void OnReset(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            LevelManager.instance.RestartLevel();
        }
    }
    #endregion
}
