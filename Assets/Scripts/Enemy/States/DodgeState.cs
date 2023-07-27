using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : State
{
    protected D_DodgeState stateData;
    protected bool _performCloseRangeAction;
    protected bool _isPlayerInMaxAgroRange;
    protected bool _isGrounded;
    protected bool _isDodgeOver;
    public DodgeState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DodgeState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        _isPlayerInMaxAgroRange = entity.CheckPlayerInMaxAgroRange();
        _isGrounded = core._collisionSenses.Grounded;
    }

    public override void Enter()
    {
        base.Enter();
        _isDodgeOver = false;
        core._movement.SetVelocity(stateData._dodgeSpeed, stateData._dodgeAngle, -core._movement._facingDirection);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(Time.time >= startTime + stateData._dodgeTime && _isGrounded)
        {
            _isDodgeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
