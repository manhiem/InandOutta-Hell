using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    protected D_MoveState statedata;

    protected bool _isDetectingWall;
    protected bool _isDetectingLedge;
    protected bool _isPlayerInMinAgroRange;
    public MoveState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState statedata) : base(entity, stateMachine, animBoolName)
    {
        this.statedata = statedata;
    }

    public override void DoChecks()
    {
        base.DoChecks();
        _isDetectingLedge = core._collisionSenses.LedgeVertical;
        _isDetectingWall = core._collisionSenses.WallFront;
        _isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
    }

    public override void Enter()
    {
        base.Enter();
        core._movement.SetVelocityX(statedata._movementSpeed * core._movement._facingDirection);


    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        core._movement.SetVelocityX(statedata._movementSpeed * core._movement._facingDirection);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
