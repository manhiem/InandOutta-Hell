using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newMeleeAttackStateData", menuName = "Data/State Data/Melee Attack State")]
public class D_MeleeAttackState : ScriptableObject
{
    public float _attackRadius = 0.5f;
    public LayerMask whatIsPlayer;
    public float attackDamage = 10;
}
