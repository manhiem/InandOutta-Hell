using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    protected D_IdleState stateData;

    protected bool _flipAfterIdle;

    protected float _idleTime;
    protected bool _isIdleTimeOver;
    protected bool _isPlayerInMinAgroRange;
    public IdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData) : base(entity, stateMachine, animBoolName)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        _isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();

        core._movement.SetVelocityX(0);
        _isIdleTimeOver = false;
        SetRandomIdleTime();

    }

    public override void Exit()
    {
        base.Exit();

        if(_flipAfterIdle)
        {
            core._movement.Flip();
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        core._movement.SetVelocityX(0);
        if (Time.time >= startTime + _idleTime)
        {
            _isIdleTimeOver = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public  void SetFlipAfterIdle(bool flip)
    {
        _flipAfterIdle = flip;
    }

    private void SetRandomIdleTime()
    {
        _idleTime = Random.Range(stateData._maxIdleTime, stateData._maxIdleTime);
    }
}
