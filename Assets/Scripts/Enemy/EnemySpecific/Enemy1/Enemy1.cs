using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Entity
{
    public E1_IdleState idleState { get; private set; }
    public E1_MoveState moveState { get; private set; }
    public E1_PlayerDetected playerDetectedState { get; private set; }
    public E1_ChargeState chargeState { get; private set; }

    [SerializeField]
    private D_IdleState idleStatedata;
    [SerializeField] 
    private D_MoveState moveStatedata;
    [SerializeField]
    private D_PlayerDetected playerDetecteddata;
    [SerializeField]
    private D_ChargeState chargeStatedata;

    public override void Start()
    {
        base.Start();

        moveState = new E1_MoveState(this, stateMachine, "Move", moveStatedata, this);
        idleState = new E1_IdleState(this, stateMachine, "Idle", idleStatedata, this);
        playerDetectedState = new E1_PlayerDetected(this, stateMachine, "PlayerDetected", playerDetecteddata, this);
        chargeState = new E1_ChargeState(this, stateMachine, "Charge", chargeStatedata, this);

        stateMachine.Initialize(moveState);
    }
}
