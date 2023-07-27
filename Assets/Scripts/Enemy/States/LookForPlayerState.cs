using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookForPlayerState : State
{
    protected D_LookForPlayerState stateData;
    protected bool _isPlayerInMinAgroRange;
    protected bool _isAllTurnsDone;
    protected bool _isAllTurnsTimeDone;
    protected bool _turnImmediately;

    protected float _lastTurnTime;
    protected int _amountOfTurnsDone;
    public LookForPlayerState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_LookForPlayerState stateData) : base(entity, stateMachine, animBoolName)
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
        _isAllTurnsDone = false;
        _isAllTurnsTimeDone = false;
        _lastTurnTime = startTime;
        _amountOfTurnsDone = 0;
        core._movement.SetVelocityX(0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        core._movement.SetVelocityX(0);

        if (_turnImmediately)
        {
            core._movement.Flip();
            _lastTurnTime = Time.time;
            _amountOfTurnsDone++;
            _turnImmediately = false;
        }
        else if(Time.time > _lastTurnTime + stateData._timeBetweenTurns && !_isAllTurnsDone) 
        {
            core._movement.Flip();
            _lastTurnTime = Time.time;
            _amountOfTurnsDone++;
        }

        if(_amountOfTurnsDone >= stateData._amountOfTurns)
        {
            _isAllTurnsDone = true;
        }

        if(Time.time >= _lastTurnTime + stateData._timeBetweenTurns && _isAllTurnsDone)
        {
            _isAllTurnsTimeDone = true;
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public void SetTurnImmediately(bool flip)
    {
        _turnImmediately = flip;
    }
}
