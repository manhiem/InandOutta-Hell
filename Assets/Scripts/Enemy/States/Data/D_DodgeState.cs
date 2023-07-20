using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDodgeStateData", menuName = "Data/State Data/Dodge State State")]
public class D_DodgeState : ScriptableObject
{
    public float _dodgeSpeed = 10f;
    public Vector2 _dodgeAngle;
    public float _dodgeTime = 0.2f;
    public float _dodgeCoolDown = 2f;
}
