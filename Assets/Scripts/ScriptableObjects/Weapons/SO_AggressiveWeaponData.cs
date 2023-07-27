using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Aggressive Weapon Data",  menuName = "Data/Weapon Data/Aggressive Weapon")]
public class SO_AggressiveWeaponData : SO_WeaponData
{
    [SerializeField]
    private WeaponAttackDetails[] attackDetails;

    public WeaponAttackDetails[] AttackDetails { get => attackDetails; set => attackDetails = value; }

    private void OnEnable()
    {
        _amountOfAttacks = attackDetails.Length;
        _movementSpeed = new float[_amountOfAttacks];

        for (int i = 0; i < _amountOfAttacks; i++)
        {
            _movementSpeed[i] = attackDetails[i]._movementSpeed; 
        }
    }
}
