using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeaponAttackDetails
{
    public string attackName;
    public float _movementSpeed;
    public float _damageAmount;

    public float _knockbackStrength;
    public Vector2 _knockbackAngle;
}