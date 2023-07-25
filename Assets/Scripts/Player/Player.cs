using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region State Variables
    public PlayerStateMachine StateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerLandState landState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallGrabState wallGrabState { get; private set; }
    public PlayerWallClimbState wallClimbState { get; private set;}
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerLedgeClimbState ledgeClimbState { get; private set; }
    public PlayerDashState dashState { get; private set; }

    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Components
    public Animator _anim { get; private set; }
    public PlayerInputHandler inputHandler { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Transform dashDirectionIndicator { get; private set; }
    #endregion

    #region Check Transforms

    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform wallCheck;
    [SerializeField]
    private Transform ledgeCheck;

    #endregion

    #region Other Variables
    public Vector2 _curVelocity { get; private set; }
    public int _facingDirection { get; private set; }


    private Vector2 _velocityWorkspace;
    #endregion



    #region Unity Callback Functions
    private void Awake()
    {
        StateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, StateMachine, playerData, "Idle");
        moveState = new PlayerMoveState(this, StateMachine, playerData, "Move");
        jumpState = new PlayerJumpState(this, StateMachine, playerData, "InAir");
        airState = new PlayerAirState(this, StateMachine, playerData, "InAir");
        landState = new PlayerLandState(this, StateMachine, playerData, "Land");
        wallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "Wall Slide");
        wallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "Wall Grab");
        wallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "Wall Climb");
        wallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "InAir");
        ledgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "Ledge Climb State");
        dashState = new PlayerDashState(this, StateMachine, playerData, "InAir");
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
        inputHandler = GetComponent<PlayerInputHandler>();
        rb = GetComponent<Rigidbody2D>();
        StateMachine.Initialize(idleState);
        _facingDirection = 1;

        dashDirectionIndicator = transform.Find("Dash Direction Indicator");
    }

    private void Update()
    {
        _curVelocity = rb.velocity;
        StateMachine.currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.currentState.PhysicsUpdate();
    }
    #endregion

    #region Set Functions
    public void SetVelocityX(float velocity)
    {
        _velocityWorkspace.Set(velocity, _curVelocity.y);
        rb.velocity = _velocityWorkspace;
        _curVelocity = _velocityWorkspace;
    }

    public void SetVelocityY(float velocity)
    {
        _velocityWorkspace.Set(_curVelocity.x, velocity);
        rb.velocity = _velocityWorkspace;
        _curVelocity = _velocityWorkspace;
    }

    public void SetVelocity(float velocity, Vector2 angle, int _direction)
    {
        angle.Normalize();
        _velocityWorkspace.Set(angle.x * velocity * _direction, angle.y * velocity);
        rb.velocity = _velocityWorkspace;
        _curVelocity = _velocityWorkspace;
    }

    public void SetVelocity0()
    {
        rb.velocity = Vector2.zero;
        _curVelocity = Vector2.zero;
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        _velocityWorkspace = velocity * direction;
        rb.velocity = _velocityWorkspace;
        _curVelocity = _velocityWorkspace;
    }

    #endregion

    #region Check Functions

    public bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, playerData._groundCheckRadius, playerData.whatIsGround);
    }

    public void CheckIfShouldFlip(int xInput)
    {
        if(xInput != 0 && xInput != _facingDirection)
        {
            Flip();
        }
    }

    public bool CheckIfTouchingLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.right * _facingDirection, playerData._wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWall()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * _facingDirection, playerData._wallCheckDistance, playerData.whatIsGround);
    }

    public bool CheckIfTouchingWallBack()
    {
        return Physics2D.Raycast(wallCheck.position, Vector2.right * -_facingDirection, playerData._wallCheckDistance, playerData.whatIsGround);
    }
    #endregion

    #region OtherFunctionns

    private void AnimationTrigger()
    {
        StateMachine.currentState.AnimationTrigger();
    }

    private void AnimationFinishTrigger()
    {
        StateMachine.currentState.AnimationFinishTrigger();
    }

    private void Flip()
    {
        _facingDirection *= -1;
        transform.Rotate(0.0f, 180f, 0.0f);
    }

    public Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(wallCheck.position, Vector2.right * _facingDirection, playerData._wallCheckDistance, playerData.whatIsGround);
        float xDistance = xHit.distance;
        _velocityWorkspace.Set(xDistance * _facingDirection, 0);

        RaycastHit2D yHit = Physics2D.Raycast(ledgeCheck.position + (Vector3)_velocityWorkspace, Vector2.down, ledgeCheck.position.y - wallCheck.position.y, playerData.whatIsGround);
        float yDistance = yHit.distance;

        _velocityWorkspace.Set(wallCheck.position.x + (xDistance *  _facingDirection), ledgeCheck.position.y - yDistance);
        return _velocityWorkspace;
    }
    #endregion
}
