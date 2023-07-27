using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallClimbState : PlayerTouchingWallState
{
    public PlayerWallClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!_isExitingState)
        {
            _core._movement.SetVelocityY(playerData._wallClimbVelocity);

            if (_yInput != 1)
            {
                stateMachine.ChangeState(player.wallGrabState);
            }
        }
    }
}
