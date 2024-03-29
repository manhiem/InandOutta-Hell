using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    // Remove Later
    private AttackDetails attackDetails;

    // Remove above part

    [SerializeField]
    private bool combatEnabled;
    [SerializeField]
    private float _inputTimer, attack1Radius, attack1Damage;
    [SerializeField]
    private Transform attack1HitBox;
    [SerializeField]
    private LayerMask whatIsDamagable;
    [SerializeField]
    private float _stunDamageAmount = 1f;

    private bool _gotInput, _isAttacking, _firstAttack = false;
    private float _lastInputTime = Mathf.NegativeInfinity;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnabled);
    }

    private void Update()
    {
        CheckCombatInput();
        CheckAttacks();
    }

    private void CheckCombatInput()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(combatEnabled)
            {
                _gotInput = true;
                _lastInputTime = Time.time;
            }
        }
    }

    private void CheckAttacks()
    {
        if (_gotInput)
        {
            // Perform Attack1
            if (!_isAttacking)
            {
                _gotInput = false;
                _isAttacking = true;
                _firstAttack = !_firstAttack;
                anim.SetBool("attack1", true);
                anim.SetBool("firstAttack", _firstAttack);
                anim.SetBool("isAttacking", _isAttacking);
            }
        }

        if (Time.time >= _lastInputTime + _inputTimer)
        {
            _gotInput = false;
        }
    }

    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBox.position, attack1Radius, whatIsDamagable);

        attackDetails.damageAmount = 20;
        attackDetails.position = attack1HitBox.position;
        attackDetails.stunDamageAmount = _stunDamageAmount;

        foreach (Collider2D collider in detectedObjects)
        {
            collider.transform.parent.SendMessage("Damage", attackDetails);
            // Instantiate Hit Particle
        }
    }

    private void FinishAttack1()
    {
        _isAttacking = false;
        anim.SetBool("isAttacking", _isAttacking);
        anim.SetBool("attack1", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBox.position, attack1Radius);
    }

    public void damage(float damage)
    {
        Debug.LogError("Got hit by: " + damage);
    } 
}
