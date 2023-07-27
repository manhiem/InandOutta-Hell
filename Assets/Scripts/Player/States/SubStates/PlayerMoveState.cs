using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        player.dustEmmission.rateOverTime = 0f;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        _core._movement.CheckIfShouldFlip(_xInput);
        _core._movement.SetVelocityX(playerData._moveSpeed * _xInput);
        player.dustEmmission.rateOverTime = 35f;
        if(_xInput == 0 && !_isExitingState)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
