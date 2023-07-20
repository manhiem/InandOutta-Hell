using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newStunStateData", menuName = "Data/State Data/Stun State")]
public class D_StunState : ScriptableObject
{
    public float _stunTime = 3f;
    public float _stunKnockBackTime = 0.2f;
    public Vector2 _stunKnockBackAngle;
    public float _stunKnockBackSpeed = 20f;
}
