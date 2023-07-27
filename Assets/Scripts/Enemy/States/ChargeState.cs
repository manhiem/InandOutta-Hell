using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeState : State
{
    protected D_ChargeState stateData;
    protected bool _isPlayerInMinAgroRange;
    protected bool _isDetectingLedge;
    protected bool _isDetectingWall;
    protected bool _isChargeTimeOver;
    protected bool _performShortRangeAction;
    public ChargeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_ChargeState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        _isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
        _isDetectingLedge = core._collisionSenses.LedgeVertical;
        _isDetectingWall = core._collisionSenses.WallFront;
        _isChargeTimeOver = false;

        _performShortRangeAction = entity.CheckPlayerInCloseRangeAction();
    }

    public override void Enter()
    {
        base.Enter();

        core._movement.SetVelocityX(stateData._chargeSpeed * core._movement._facingDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        core._movement.SetVelocityX(stateData._chargeSpeed * core._movement._facingDirection);
        if (Time.time >= startTime + stateData._chargetime)
        {
            _isChargeTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
