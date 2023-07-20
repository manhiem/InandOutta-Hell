using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : AttackState
{
    protected D_RangedAttackState stateData;
    protected GameObject _projectile;
    protected Projectile _projectileScript;
    public RangedAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform _attackPosition, D_RangedAttackState stateData) : base(entity, stateMachine, animBoolName, _attackPosition)
    {
        this.stateData = stateData;
    }

    public override void DoChecks()
    {
        base.DoChecks();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FinishAttack()
    {
        base.FinishAttack();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void TriggerAttack()
    {
        base.TriggerAttack();

        _projectile = GameObject.Instantiate(stateData.projectilePrefab, _attackPosition.position, _attackPosition.rotation);
        _projectileScript = _projectile.GetComponent<Projectile>();
        _projectileScript.FireProjectile(stateData._projectileSpeed, stateData._projectileTravelDistance, stateData._projectiledamage);
    }
}
