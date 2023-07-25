using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallGrabState : PlayerTouchingWallState
{
    private Vector2 _holdPosition;
    
    public PlayerWallGrabState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) 
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
    }

    public override void Enter()
    {
        base.Enter();
        _holdPosition = player.transform.position;

        HoldPosition();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!_isExitingState) 
        {

            HoldPosition();

            if (_yInput > 0)
            {
                stateMachine.ChangeState(player.wallClimbState);
            }
            else if(_yInput < 0 || !_grabInput)
            {
                stateMachine.ChangeState(player.wallSlideState);
            }
        }

    }

    private void HoldPosition()
    {
        player.transform.position = _holdPosition;
        player.SetVelocityX(0);
        player.SetVelocityY(0);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
