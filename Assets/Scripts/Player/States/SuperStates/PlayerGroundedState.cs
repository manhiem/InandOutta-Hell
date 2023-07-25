using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int _xInput;

    private bool _jumpInput;
    private bool _isGrounded;
    private bool _isTouchingWall;
    private bool _grabInput;
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _isGrounded = player.CheckIfGrounded();
        _isTouchingWall = player.CheckIfTouchingWall();
    }

    public override void Enter()
    {
        base.Enter();
        player.jumpState.ResetAmountOfJumps();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        _xInput = player.inputHandler._normalizedInputX;
        _jumpInput = player.inputHandler._jumpInput;
        _grabInput = player.inputHandler._grabInput;

        if(_jumpInput && player.jumpState.CanJump())
        {
            stateMachine.ChangeState(player.jumpState);
        }
        else if(!_isGrounded)
        {
            player.airState.StartCoyoteTime();
            stateMachine.ChangeState(player.airState);
        }
        else if(_isTouchingWall && _grabInput)
        {
            stateMachine.ChangeState(player.wallGrabState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
