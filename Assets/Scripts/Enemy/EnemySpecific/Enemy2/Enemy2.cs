using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Entity
{
    public E2_MoveState moveState {  get; private set; }
    public E2_IdleState idleState { get; private set; }
    public E2_PlayerDetectedState playerDetectedState { get; private set; }
    public E2_MeleeAttackState meleeAttackState { get; private set; }
    public E2_LookForPlayerState lookForPlayerState { get; private set; }
    public E2_StunState stunState { get; private set; }
    public E2_DeadState deadState { get; private set; }

    [SerializeField]
    private D_MoveState moveStateData;
    [SerializeField] 
    private D_IdleState idleStateData;
    [SerializeField]
    private D_PlayerDetected playerDetectedStateData;
    [SerializeField]
    private D_MeleeAttackState meleeAttackStateData;
    [SerializeField]
    private D_LookForPlayerState lookForPlayerStateData;
    [SerializeField]
    private D_StunState stunStateData;
    [SerializeField] 
    private D_DeadState deadStateData;

    [SerializeField]
    private Transform meleeAttackPosition;

    public override void Start()
    {
        base.Start();
        moveState = new E2_MoveState(this, stateMachine, "Move", moveStateData, this);
        idleState = new E2_IdleState(this, stateMachine, "Idle", idleStateData, this);
        playerDetectedState = new E2_PlayerDetectedState(this, stateMachine, "Player Detected", playerDetectedStateData, this);
        meleeAttackState = new E2_MeleeAttackState(this, stateMachine, "Melee Attack", meleeAttackPosition, meleeAttackStateData, this);
        lookForPlayerState = new E2_LookForPlayerState(this, stateMachine, "Look For Player", lookForPlayerStateData, this);
        stunState = new E2_StunState(this, stateMachine, "Stun", stunStateData, this);
        deadState = new E2_DeadState(this, stateMachine, "Dead", deadStateData, this);

        stateMachine.Initialize(moveState);
    }

    public override void Damage(AttackDetails attackDetails)
    {
        base.Damage(attackDetails);

        if(_isDead)
        {
            stateMachine.ChangeState(deadState);
        }
        else if(_isStunned && stateMachine.currentState!=stunState)
        {
            stateMachine.ChangeState(stunState);
        }
        else if(!CheckPlayerInMinAgroRange())
        {
            lookForPlayerState.SetTurnImmediately(true);
            stateMachine.ChangeState(lookForPlayerState);
        }
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData._attackRadius);
    }
}
