using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLandState : PlayerGroundedState
{
    public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player._jumpParticles.gameObject.SetActive(true);
        player._jumpParticles.Stop();
        player._jumpParticles.Play();
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
            if (_xInput != 0)
            {
                stateMachine.ChangeState(player.moveState);
            }
            else if (_isAnimationFinished)
            {
                stateMachine.ChangeState(player.idleState);
            }
        }

    }
}
