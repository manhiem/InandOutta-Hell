using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data",  menuName ="Data/Weapon Data/Weapon")]
public class SO_WeaponData : ScriptableObject
{
    public int _amountOfAttacks { get; protected set; }
    public float[] _movementSpeed {  get; protected set; }
}
