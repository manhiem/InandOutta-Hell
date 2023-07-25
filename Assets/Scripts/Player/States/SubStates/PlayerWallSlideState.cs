using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSlideState : PlayerTouchingWallState
{
    public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!_isExitingState)
        {
            player.SetVelocityY(-playerData._wallSlideVelocity);

            if (_grabInput && _yInput == 0f)
            {
                stateMachine.ChangeState(player.wallGrabState);
            }
        }
    }
}
