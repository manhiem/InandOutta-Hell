using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectedState : State
{
    protected D_PlayerDetected stateData;
    protected bool _isPlayerInMinAgroRange;
    protected bool _isPlayerInMaxAgroRange;
    protected bool _performLongRangeAction;
    protected bool _performShortRangeAction;
    protected bool _isDetectingLedge;

    public PlayerDetectedState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetected stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        _isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        _isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();

        _performShortRangeAction = entity.CheckPlayerInCloseRangeAction();
        _isDetectingLedge = core._collisionSenses.LedgeVertical;
    }

    public override void Enter()
    {
        base.Enter();
        core._movement.SetVelocityX(0);
        _performLongRangeAction = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        core._movement.SetVelocityX(0);
        if (Time.time >= startTime + stateData._longRangeActionTime)
        {
            _performLongRangeAction = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
