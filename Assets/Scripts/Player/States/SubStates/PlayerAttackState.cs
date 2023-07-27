using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    private Weapon _weapon;
    private float _velocityToSet;
    private bool _setVelocity;
    private int _xInput;
    private bool _shouldCheckFlip;
    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        _weapon.EnterWeapon();
    }

    public override void Exit() 
    { 
        base.Exit();
        _setVelocity = false;
        _weapon.ExitWeapon();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        _xInput = player.inputHandler._normalizedInputX;

        if(_shouldCheckFlip)
        _core._movement.CheckIfShouldFlip(_xInput);

        if(_setVelocity )
        {
            _core._movement.SetVelocityX(_velocityToSet * _core._movement._facingDirection);
        }
    }

    public void SetWeapon(Weapon weapon)
    {
        this._weapon = weapon;
        weapon.InitializeWeapon(this, _core);
    }

    public void SetPlayerVelocity(float velocity)
    {
        _core._movement.SetVelocityX(velocity * _core._movement._facingDirection);

        _velocityToSet = velocity;
        _setVelocity = true;
    }

    public void SetFlipCheck(bool value)
    {
        _shouldCheckFlip = value;
    }

    #region Animation Triggers
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();

        _isAbilityDone = true;
    }

    #endregion
}
