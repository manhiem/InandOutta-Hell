using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="newEntityData", menuName = "Data/Entity Data/Base Data")]
public class D_Entity : ScriptableObject
{
    public float _wallCheckDistance = 0.2f;
    public float _ledgeCheckDistance = 0.4f;

    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    public float _maxAgroDistance = 4f;
    public float _minAgroDistance = 3f;

    public float _closeRangeActionDistance = 1f;

    public float _maxHealth = 120f;
    public float _damageHopSpeed = 3f;
    public float _groundCheckRadius = 0.3f;

    public float _stunResistance = 3f;
    public float _stunRecoveryTime = 2f;

    public GameObject hitParticle;
}
