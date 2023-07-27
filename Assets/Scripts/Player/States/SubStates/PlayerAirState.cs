using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    //Input
    private int _xInput;
    private bool _jumpInput;
    private bool _grabInput;
    private bool _dashInput;


    //Checks
    private bool _isGrounded;
    private bool _jumpInputStop;
    private bool _isTouchingWall;
    private bool _isTouchingWallBack;
    private bool _oldIsTouchingWall;
    private bool _oldIsTouchingWallBack;
    private bool _isTouchingLedge;
    private float _startWallJumpCoyoteTime;

    private bool _coyoteTime;
    private bool _wallJumpCoyoteTime;
    private bool _isJumping;

    public PlayerAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _oldIsTouchingWall = _isTouchingWall;
        _oldIsTouchingWallBack = _isTouchingWallBack;

        _isGrounded = player._core._collisionSenses.Grounded;
        _isTouchingWall = player._core._collisionSenses.WallFront;
        _isTouchingWallBack = player._core._collisionSenses.WallBack;
        _isTouchingLedge = player._core._collisionSenses.LedgeHorizontal;

        if(_isTouchingWall && !_isTouchingLedge)
        {
            player.ledgeClimbState.SetDetectedPosition(player.transform.position);
        }

        if(!_wallJumpCoyoteTime && !_isTouchingWall && !_isTouchingWallBack && (_oldIsTouchingWall || _oldIsTouchingWallBack))
        {
            StartWallJumpCoyoteTime();
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        _oldIsTouchingWall = false;
        _oldIsTouchingWallBack = false;
        _isTouchingWall = false;
        _isTouchingWallBack = false;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();

        _xInput = player.inputHandler._normalizedInputX;
        _jumpInput = player.inputHandler._jumpInput;
        _jumpInputStop = player.inputHandler._holdJumpInputStop;
        _grabInput = player.inputHandler._grabInput;
        _dashInput = player.inputHandler._dashInput;

        CheckJumpMultiplier();

        if (player.inputHandler._attackInputs[(int)CombatInputs.primary])
        {
            stateMachine.ChangeState(player.primaryAttackState);
        }
        else if (player.inputHandler._attackInputs[(int)CombatInputs.secondary])
        {
            stateMachine.ChangeState(player.secondaryAttackState);
        }
        else if (_isGrounded && _core._movement._curVelocity.y < 0.01f)
        {
            stateMachine.ChangeState(player.landState);
        }
        else if(_isTouchingWall && !_isTouchingLedge && !_isGrounded)
        {
            stateMachine.ChangeState(player.ledgeClimbState);
        }
        else if(_jumpInput && (_isTouchingWall || _isTouchingWallBack || _wallJumpCoyoteTime))
        {
            StopWallJumpCoyoteTime();
            _isTouchingWall = player._core._collisionSenses.WallFront;
            player.wallJumpState.DeterminewallJumpDirection(_isTouchingWall);
            stateMachine.ChangeState(player.wallJumpState);
        }
        else if(_jumpInput && player.jumpState.CanJump())
        {
            stateMachine.ChangeState(player.jumpState);
        }
        else if(_isTouchingWall && _grabInput && _isTouchingLedge)
        {
            stateMachine.ChangeState(player.wallGrabState);
        }
        else if(_isTouchingWall && _xInput == _core._movement._facingDirection && _core._movement._curVelocity.y <= 0f)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }
        else if(_dashInput && player.dashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.dashState);
        }
        else
        {
            _core._movement.CheckIfShouldFlip(_xInput);
            _core._movement.SetVelocityX(playerData._moveSpeed * _xInput);

            player._anim.SetFloat("yVelocity", _core._movement._curVelocity.y);
            player._anim.SetFloat("xVelocity", Mathf.Abs(_core._movement._curVelocity.x));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void CheckCoyoteTime()
    {
        if (_coyoteTime && Time.time > _startTime + playerData._coyoteTime)
        {
            _coyoteTime = false;
            player.jumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    private void CheckJumpMultiplier()
    {
        if (_isJumping)
        {
            if (_jumpInputStop)
            {
                _core._movement.SetVelocityY(_core._movement._curVelocity.y * playerData._jumpHeightMultiplier);
                _isJumping = false;
            }
            else if (_core._movement._curVelocity.y <= 0)
            {
                _isJumping = false;
            }
        }
    }

    public void StartCoyoteTime() => _coyoteTime = true;
    public void StartWallJumpCoyoteTime()
    {
        _wallJumpCoyoteTime = true;
        _startWallJumpCoyoteTime = Time.time;
    }

    public void StopWallJumpCoyoteTime() => _wallJumpCoyoteTime = false;
    public void CheckWallJumpCoyoteTime()
    {
        if(_wallJumpCoyoteTime && Time.time > _startWallJumpCoyoteTime + playerData._coyoteTime)
        {
            _wallJumpCoyoteTime = false;
        }
    }
    public void SetIsJumping() => _isJumping = true;
}
