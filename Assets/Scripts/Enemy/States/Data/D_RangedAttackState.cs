using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newRangedAttackStateData", menuName = "Data/State Data/Ranged Attack State State")]
public class D_RangedAttackState : ScriptableObject
{
    public GameObject projectilePrefab;
    public float _projectiledamage = 10f;
    public float _projectileSpeed = 12f;
    public float _projectileTravelDistance;
}
