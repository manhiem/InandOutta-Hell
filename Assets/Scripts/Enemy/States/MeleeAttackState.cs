using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeAttackState : AttackState
{
    protected D_MeleeAttackState stateData;
    public MeleeAttackState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform _attackPosition, D_MeleeAttackState stateData) : base(entity, stateMachine, animBoolName, _attackPosition)
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

        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(_attackPosition.position, stateData._attackRadius, stateData.whatIsPlayer);

        foreach (Collider2D collider in detectedObjects)
        {
            Idamagable damagable = collider.GetComponent<Idamagable>();

            if(damagable != null)
            {
                damagable.Damage(stateData.attackDamage);
            }

            IKnockbackable knockbackable = collider.GetComponent<IKnockbackable>();
            if(knockbackable != null)
            {
                knockbackable.Knockback(stateData._knockbackangle, stateData._knockbackStrength, core._movement._facingDirection);
            }
        }
    }
}
