using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    protected Transform _attackPosition;
    protected bool _isAnimationFinished;
    protected bool _isPlayerInMinAgroRange;

    public AttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform _attackPosition) : base(entity, stateMachine, animBoolName)
    {
        this._attackPosition = _attackPosition;
    }

    public override void DoChecks()
    {
        base.DoChecks();

        _isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        entity._animationToStateMachine.attackState = this;
        _isAnimationFinished = false;
        entity.SetVelocity(0f);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public virtual void TriggerAttack()
    {

    }

    public virtual void FinishAttack()
    {
        _isAnimationFinished = true;
    }
}
