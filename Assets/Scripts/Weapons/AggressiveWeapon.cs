using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveWeapon : Weapon
{
    protected SO_AggressiveWeaponData _aggressiveWeaponData;

    private List<Idamagable> _detectedDamagables = new List<Idamagable>();

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
        foreach (Idamagable item in _detectedDamagables)
        {
            item.Damage(details._damageAmount);
        }
    }

    public void AddToDetected(Collider2D collision)
    {
        Idamagable damagable = collision.GetComponent<Idamagable>();

        if (damagable != null)
        {
            _detectedDamagables.Add(damagable);
        }
    }

    public void RemoveFromDetected(Collider2D collision)
    {
        Idamagable damagable = collision.GetComponent<Idamagable>();

        if (damagable != null)
        {
            _detectedDamagables.Remove(damagable);
        }
    }
}
