using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : CoreComponent, Idamagable, IKnockbackable
{
    [SerializeField]
    private float _maxKnockbackTime = 0.2f;
    private bool _isKnockbackActive;
    private float _knockbackStartTime;

    public override void LogicUpdate()
    {
        CheckKnockback();
    }

    public void Damage(float amount)
    {
        _core._stats.DecreaseHealth(amount);
        Debug.Log(_core.transform.parent.name + " damaged");
    }

    public void Knockback(Vector2 angle, float strength, int direction)
    {
        _core._movement.SetVelocity(strength, angle, direction);
        _core._movement._canSetVelocity = false;
        _isKnockbackActive = true;
        _knockbackStartTime = Time.time;
    }

    private void CheckKnockback()
    {
        if(_isKnockbackActive && (_core._movement._curVelocity.y <= 0.01f && _core._collisionSenses.Grounded)
            || Time.time >= _knockbackStartTime + _maxKnockbackTime)
        {
            _isKnockbackActive=false;
            _core._movement._canSetVelocity = true;
        }
    }
}
