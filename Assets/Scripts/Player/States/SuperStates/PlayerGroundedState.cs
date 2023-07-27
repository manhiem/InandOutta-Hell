using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int _xInput;
    protected bool _dashInput;
    private bool _grabInput;
    private bool _jumpInput;

    private bool _isGrounded;
    private bool _isTouchingWall;
    private bool _isTouchingLedge;
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _isGrounded = player._core._collisionSenses.Grounded;
        _isTouchingWall = player._core._collisionSenses.WallFront;
        _isTouchingLedge = player._core._collisionSenses.Ledge;
    }

    public override void Enter()
    {
        base.Enter();
        player.jumpState.ResetAmountOfJumps();
        player.dashState.ResetCanDash();
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
        _dashInput = player.inputHandler._dashInput;

        if (player.inputHandler._attackInputs[(int)CombatInputs.primary])
        {
            stateMachine.ChangeState(player.primaryAttackState);
        }
        else if (player.inputHandler._attackInputs[(int)CombatInputs.secondary])
        {
            stateMachine.ChangeState(player.secondaryAttackState);
        }
        else if (_jumpInput && player.jumpState.CanJump())
        {
            stateMachine.ChangeState(player.jumpState);
        }
        else if(!_isGrounded)
        {
            player.airState.StartCoyoteTime();
            stateMachine.ChangeState(player.airState);
        }
        else if(_isTouchingWall && _grabInput && _isTouchingLedge)
        {
            stateMachine.ChangeState(player.wallGrabState);
        }
        else if (_dashInput && player.dashState.CheckIfCanDash())
        {
            stateMachine.ChangeState(player.dashState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
