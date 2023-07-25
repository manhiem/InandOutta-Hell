using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerAbilityState
{
    private int _wallJumpDirection;
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.inputHandler.UseJumpInput();
        player.jumpState.ResetAmountOfJumps();
        player.SetVelocity(playerData._wallJumpVelocity, playerData._wallJumpAngle, _wallJumpDirection);
        player.CheckIfShouldFlip(_wallJumpDirection);
        player.jumpState.DecreaseAmountOfJumpsLeft();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        player._anim.SetFloat("yVelocity", player._curVelocity.y);
        player._anim.SetFloat("xVelocity", Mathf.Abs(player._curVelocity.x));

        if(Time.time >= _startTime + playerData._wallJumpTime)
        {
            _isAbilityDone = true;
        }
    }

    public void DeterminewallJumpDirection(bool _isTouchingWall)
    {
        if(_isTouchingWall)
        {
            _wallJumpDirection = -player._facingDirection;
        }
        else
        {
            _wallJumpDirection = player._facingDirection;
        }
    }
}
