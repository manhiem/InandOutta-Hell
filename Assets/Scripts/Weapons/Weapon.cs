using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    protected SO_WeaponData _weaponData;

    [SerializeField]
    private Animator _baseAnimator;
    [SerializeField]
    private Animator _weaponAnimator;

    protected PlayerAttackState _state;
    protected int _attackCounter;

    protected virtual void Awake()
    {
        gameObject.SetActive(false);
    }

    public virtual void EnterWeapon()
    {
        gameObject.SetActive(true);

        if(_attackCounter>=_weaponData._amountOfAttacks)
        {
            _attackCounter = 0;
        }

        _baseAnimator.SetInteger("Attack Counter", _attackCounter);
        _weaponAnimator.SetInteger("Attack Counter", _attackCounter);

        _baseAnimator.SetBool("Attack", true);
        _weaponAnimator.SetBool("Attack", true);   
    }

    public virtual void ExitWeapon()
    {
        _baseAnimator.SetBool("Attack", false);
        _weaponAnimator.SetBool("Attack", false);
        _attackCounter++;

        gameObject.SetActive(false);
    }

    public void InitializeWeapon(PlayerAttackState state)
    {
        _state = state;
    }

    #region Animation Triggers

    public virtual void AnimationFinishTrigger()
    {
        _state.AnimationFinishTrigger();
    }

    public virtual void AnimationStartMovementTrigger()
    {
        _state.SetPlayerVelocity(_weaponData._movementSpeed[_attackCounter]);
    }
    public virtual void AnimationStopMovementTrigger()
    {
        _state.SetPlayerVelocity(0f);
    }

    public virtual void AnimationTurnOffFlipTrigger()
    {
        _state.SetFlipCheck(false);
    }

    public virtual void AnimationTurnOnFlipTrigger()
    {
        _state.SetFlipCheck(true);
    }

    public virtual void AnimationActionTrigger()
    {

    }
    #endregion


}
