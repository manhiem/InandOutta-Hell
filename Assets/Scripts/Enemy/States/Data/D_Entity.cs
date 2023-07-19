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
}
