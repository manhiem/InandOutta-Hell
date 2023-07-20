using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : State
{
    protected D_StunState stateData;
    protected bool _isStunTimeOver;
    protected bool _isGrounded;
    protected bool _isMovingStopped;

    protected bool _performCloseRangeAction;
    protected bool _isPlayerInMinAgroRange;
    public StunState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        _isGrounded = entity.CheckGround();

        _performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
        _isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        _isStunTimeOver = false;
        _isMovingStopped = false;
        entity.SetVelocity(stateData._stunKnockBackSpeed, stateData._stunKnockBackAngle, entity._lastDamageDirection);
    }

    public override void Exit()
    {
        base.Exit();
        entity.ResetStunResistance();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(Time.time >= stateData._stunTime + startTime)
        {
            _isStunTimeOver = true;
        }

        if(_isGrounded && Time.time >= startTime + stateData._stunKnockBackTime && !_isMovingStopped)
        {
            _isMovingStopped = true;
            entity.SetVelocity(0);
        } 
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
