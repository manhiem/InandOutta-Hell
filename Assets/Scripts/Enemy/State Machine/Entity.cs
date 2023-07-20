using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public FiniteStateMachine stateMachine;
    public D_Entity entityData;
    public int _facingDirection {  get; private set; }
    public Rigidbody2D rb { get; private set; }
    public Animator anim { get; private set; }
    public GameObject aliveGO { get; private set; }
    public AnimatonToStateMachine _animationToStateMachine { get; private set; }
    public int _lastDamageDirection { get; private set; }

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
    

    public virtual void Start()
    {
        _curHealth = entityData._maxHealth;
        _curStunResistance = entityData._stunResistance;
        _facingDirection = -1;
        aliveGO = transform.Find("Alive").gameObject;
        rb = aliveGO.GetComponent<Rigidbody2D>();
        anim = aliveGO.GetComponent<Animator>();
        _animationToStateMachine = aliveGO.GetComponent<AnimatonToStateMachine>();

        stateMachine = new FiniteStateMachine();
    }

    public virtual void Update()
    {
        stateMachine.currentState.LogicUpdate();
        anim.SetFloat("YVelocity", rb.velocity.y);

        if(Time.time >= _lastDamageTime + entityData._stunRecoveryTime)
        {
            ResetStunResistance();
        }
    }

    public virtual void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }

    public virtual void SetVelocity(float velocity)
    {
        _velocityWorkspace.Set(_facingDirection * velocity, rb.velocity.y);
        rb.velocity = _velocityWorkspace;
    }

    public virtual void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize();
        _velocityWorkspace.Set(angle.x * velocity * direction, angle.y * velocity);
        rb.velocity = _velocityWorkspace;
    }

    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallCheck.position, aliveGO.transform.right, entityData._wallCheckDistance, entityData.whatIsGround);
    }
    
    public virtual bool CheckLedge()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData._ledgeCheckDistance, entityData.whatIsGround);
    }

    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, -aliveGO.transform.right, entityData._minAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, -aliveGO.transform.right, entityData._maxAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, -aliveGO.transform.right, entityData._closeRangeActionDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckGround()
    {
        return Physics2D.OverlapCircle(groundCheck.position, entityData._groundCheckRadius, entityData.whatIsGround);
    }

    public virtual void ResetStunResistance()
    {
        _isStunned = false;
        _curStunResistance = entityData._stunResistance;
    }

    public virtual void Damage(AttackDetails attackDetails)
    {
        _lastDamageTime = Time.time;
        _curHealth -= attackDetails.damageAmount;
        _curStunResistance -= attackDetails.stunDamageAmount;

        DamageHop(entityData._damageHopSpeed);
        Instantiate(entityData.hitParticle, aliveGO.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0f, 360f)));

        if(_curStunResistance >= 0)
        {
            _isStunned = true;
        }

        if(attackDetails.position.x > aliveGO.transform.position.x)
        {
            _lastDamageDirection = -1;
        }
        else
        {
            _lastDamageDirection = 1;
        }

        if (_curHealth <= 0)
        {
            _isDead = true;
        }
    }

    public virtual void DamageHop(float velocity)
    {
        _velocityWorkspace.Set(rb.velocity.x , velocity);
        rb.velocity = _velocityWorkspace;
    }

    public virtual void Flip()
    {
        _facingDirection *= -1;
        aliveGO.transform.Rotate(0.0f, 180f, 0.0f);
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * _facingDirection * entityData._wallCheckDistance));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData._ledgeCheckDistance));

        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)((-Vector2.right) * entityData._closeRangeActionDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)((-Vector2.right) * entityData._minAgroDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)((-Vector2.right) * entityData._maxAgroDistance), 0.2f);
    }
}
