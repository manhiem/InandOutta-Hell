using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AggressiveWeapon : Weapon
{
    protected SO_AggressiveWeaponData _aggressiveWeaponData;

    private List<Idamagable> _detectedDamagables = new List<Idamagable>();
    private List<IKnockbackable> _detectedKnockbackables = new List<IKnockbackable>();

    protected override void Awake()
    {
        base.Awake();

        if(_weaponData.GetType() == typeof(SO_AggressiveWeaponData))
        {
            _aggressiveWeaponData = (SO_AggressiveWeaponData) _weaponData;
        }
        else
        {
            Debug.LogError("Wrong data for the weapon!");
        }
    }
    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        CheckMeleeAttack();
    }

    private void CheckMeleeAttack()
    {
        WeaponAttackDetails details = _aggressiveWeaponData.AttackDetails[_attackCounter];
        foreach (Idamagable item in _detectedDamagables.ToList())
        {
            item.Damage(details._damageAmount);
        }

        foreach (IKnockbackable item in _detectedKnockbackables.ToList())
        {
            item.Knockback(details._knockbackAngle, details._knockbackStrength, _core._movement._facingDirection);
        }
    }

    public void AddToDetected(Collider2D collision)
    {
        Idamagable damagable = collision.GetComponent<Idamagable>();

        if (damagable != null)
        {
            _detectedDamagables.Add(damagable);
        }

        IKnockbackable knockbackable = collision.GetComponent<IKnockbackable>();
        if(knockbackable != null)
        {
            _detectedKnockbackables.Add(knockbackable);
        }
    }

    public void RemoveFromDetected(Collider2D collision)
    {
        Idamagable damagable = collision.GetComponent<Idamagable>();

        if (damagable != null)
        {
            _detectedDamagables.Remove(damagable);
        }

        IKnockbackable knockbackable = collision.GetComponent<IKnockbackable>();
        if (knockbackable != null)
        {
            _detectedKnockbackables.Remove(knockbackable);
        }
    }
}
