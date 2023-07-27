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
    public PlayerAttackState primaryAttackState { get; private set; }
    public PlayerAttackState secondaryAttackState { get; private set; }

    [SerializeField]
    private PlayerData playerData;
    #endregion

    #region Components
    public Animator _anim { get; private set; }
    public PlayerInputHandler inputHandler { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Transform dashDirectionIndicator { get; private set; }
    public PlayerInventory inventory { get; private set; }
    public Core _core { get; private set; }
    #endregion

    #region Other Variables
    [SerializeField]
    private ParticleSystem _walkParticles;

    public ParticleSystem _jumpParticles;
    //public Vector2 _curVelocity { get; private set; }
    //public int _facingDirection { get; private set; }


    private Vector2 _velocityWorkspace;
    public ParticleSystem.EmissionModule dustEmmission;
    #endregion



    #region Unity Callback Functions
    private void Awake()
    {
        _core = GetComponentInChildren<Core>();

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
        primaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "Attack");
        secondaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "Attack");
        inventory = GetComponent<PlayerInventory>();

        dustEmmission = _walkParticles.emission;
    }

    private void Start()
    {
        _anim = GetComponent<Animator>();
        inputHandler = GetComponent<PlayerInputHandler>();
        rb = GetComponent<Rigidbody2D>();
        StateMachine.Initialize(idleState);
        //_facingDirection = 1;

        primaryAttackState.SetWeapon(inventory.weapons[(int)CombatInputs.primary]);
        //secondaryAttackState.SetWeapon(inventory.weapons[(int)CombatInputs.primary]);

        dashDirectionIndicator = transform.Find("Dash Direction Indicator");
    }

    private void Update()
    {
        _core.LogicUpdate();
        StateMachine.currentState.LogicUpdate();
    }

    private void FixedUpdate()
    {
        StateMachine.currentState.PhysicsUpdate();
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
    #endregion
}
