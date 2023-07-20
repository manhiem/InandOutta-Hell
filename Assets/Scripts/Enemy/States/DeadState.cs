using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeadState : State
{
    protected D_DeadState stateData;
    public DeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData) : base(entity, stateMachine, animBoolName)
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
        GameObject.Instantiate(stateData._deathBloodParticle, entity.aliveGO.transform.position, stateData._deathBloodParticle.transform.rotation);
        GameObject.Instantiate(stateData._deathChunckParticle, entity.aliveGO.transform.position, stateData._deathChunckParticle.transform.rotation);
        entity.gameObject.SetActive(false);
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
}
