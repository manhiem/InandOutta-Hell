using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Entity
{
    public E1_IdleState idleState { get; private set; }
    public E1_MoveState moveState { get; private set; }
    public E1_PlayerDetected playerDetectedState { get; private set; }
    public E1_ChargeState chargeState { get; private set; }
    public E1_LookForPlayerState lookForPlayerState { get; private set; }
    public E1_MeleeAttackState meleeAttackState { get; private set; }
    public E1_StunState stunState { get; private set; }
    public E1_DeadState deadState { get; private set; }

    [SerializeField]
    private D_IdleState idleStatedata;
    [SerializeField] 
    private D_MoveState moveStatedata;
    [SerializeField]
    private D_PlayerDetected playerDetecteddata;
    [SerializeField]
    private D_ChargeState chargeStatedata;
    [SerializeField]
    private D_LookForPlayerState lookForPlayerStateData;
    [SerializeField]
    private D_MeleeAttackState meleeAttackStateData;
    [SerializeField]
    private D_StunState stunStateData;
    [SerializeField]
    private D_DeadState deadStateData;

    [SerializeField]
    private Transform meleeAttackPosition;

    public override void Awake()
    {
        base.Awake();

        moveState = new E1_MoveState(this, stateMachine, "Move", moveStatedata, this);
        idleState = new E1_IdleState(this, stateMachine, "Idle", idleStatedata, this);
        playerDetectedState = new E1_PlayerDetected(this, stateMachine, "PlayerDetected", playerDetecteddata, this);
        chargeState = new E1_ChargeState(this, stateMachine, "Charge", chargeStatedata, this);
        lookForPlayerState = new E1_LookForPlayerState(this, stateMachine, "LookForPlayer", lookForPlayerStateData, this);
        meleeAttackState = new E1_MeleeAttackState(this, stateMachine, "MeleeAttack", meleeAttackPosition, meleeAttackStateData, this);
        stunState = new E1_StunState(this, stateMachine, "Stun", stunStateData, this);
        deadState = new E1_DeadState(this, stateMachine, "Dead", deadStateData, this);

    }

    private void Start()
    {
        stateMachine.Initialize(moveState);
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawWireSphere(meleeAttackPosition.position, meleeAttackStateData._attackRadius);
    }
}
