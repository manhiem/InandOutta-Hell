using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbilityState
{
    private int _amountOfJumpsLeft;
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
        _amountOfJumpsLeft = playerData._amountOfJumps;
    }

    public override void Enter()
    {
        base.Enter();
        player.inputHandler.UseJumpInput();
        _core._movement.SetVelocityY(playerData._jumpVelocity);
        _isAbilityDone = true;
        _amountOfJumpsLeft--;
        player.airState.SetIsJumping();
    }

    public bool CanJump()
    {
        if (_amountOfJumpsLeft > 0) return true;
        else return false;
    }

    public void ResetAmountOfJumps() => _amountOfJumpsLeft = playerData._amountOfJumps;

    public void DecreaseAmountOfJumpsLeft() => _amountOfJumpsLeft--;
}
