using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;
    public D_Entity entityData;
    public Animator anim { get; private set; }
    public AnimatonToStateMachine _animationToStateMachine { get; private set; }
    public int _lastDamageDirection { get; private set; }
    public Core _core { get; private set; }

    [SerializeField]
    private Transform wallCheck;
    [SerializeField]
    private Transform ledgeCheck;
    [SerializeField]
    private Transform playerCheck;
    [SerializeField]
    private Transform groundCheck;

    private Vector2 _velocityWorkspace;
    private float _curHealth;
    private float _curStunResistance;
    private float _lastDamageTime;

    protected bool _isStunned;
    protected bool _isDead;
    

    public virtual void Awake()
    {
        _core = GetComponentInChildren<Core>();
        _curHealth = entityData._maxHealth;
        _curStunResistance = entityData._stunResistance;
        anim = GetComponent<Animator>();
        _animationToStateMachine = GetComponent<AnimatonToStateMachine>();

        stateMachine = new FiniteStateMachine();
    }


    public virtual void Update()
    {
        _core.LogicUpdate();
        stateMachine.currentState.LogicUpdate();
        anim.SetFloat("YVelocity", _core._movement.rb.velocity.y);

        if(Time.time >= _lastDamageTime + entityData._stunRecoveryTime)
        {
            ResetStunResistance();
        }
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, -transform.right, entityData._minAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, -transform.right, entityData._maxAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, -transform.right, entityData._closeRangeActionDistance, entityData.whatIsPlayer);
    }

    public virtual void ResetStunResistance()
    {
        _isStunned = false;
        _curStunResistance = entityData._stunResistance;
    }

    public virtual void DamageHop(float velocity)
    {
        _velocityWorkspace.Set(_core._movement.rb.velocity.x , velocity);
        _core._movement.rb.velocity = _velocityWorkspace;
    }

    public virtual void OnDrawGizmos()
    {
        if(_core!=null)
        {
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * _core._movement._facingDirection * entityData._wallCheckDistance));
            Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData._ledgeCheckDistance));

            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)((-Vector2.right) * entityData._closeRangeActionDistance), 0.2f);
            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)((-Vector2.right) * entityData._minAgroDistance), 0.2f);
            Gizmos.DrawWireSphere(playerCheck.position + (Vector3)((-Vector2.right) * entityData._maxAgroDistance), 0.2f);
        }

    }
}
