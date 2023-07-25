using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchingWallState : PlayerState
{
    protected bool _isGrounded;
    protected bool _isTouchingWall;
    protected int _xInput;
    protected int _yInput;
    protected bool _grabInput;
    protected bool _jumpInput;
    protected bool _isTouchingLedge;
    public PlayerTouchingWallState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void DoChecks()
    {
        base.DoChecks();
        _isGrounded = player.CheckIfGrounded();
        _isTouchingWall = player.CheckIfTouchingWall();
        _isTouchingLedge = player.CheckIfTouchingLedge();

        if(_isTouchingWall && !_isTouchingLedge)
        {
            player.ledgeClimbState.SetDetectedPosition(player.transform.position);
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _xInput = player.inputHandler._normalizedInputX;
        _yInput = player.inputHandler._normalizedInputY;
        _grabInput = player.inputHandler._grabInput;
        _jumpInput = player.inputHandler._jumpInput;

        if(_jumpInput)
        {
            player.wallJumpState.DeterminewallJumpDirection(_isTouchingWall);
            stateMachine.ChangeState(player.wallJumpState);
        }
        else if (_isGrounded && !_grabInput)
        {
            stateMachine.ChangeState(player.idleState);
        }
        else if(!_isTouchingWall || (_xInput!=player._facingDirection && !_grabInput))
        {
            stateMachine.ChangeState(player.airState);
        }
        else if(_isTouchingWall && !_isTouchingLedge)
        {
            stateMachine.ChangeState(player.ledgeClimbState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
