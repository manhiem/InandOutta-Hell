using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDeadStateData", menuName = "Data/State Data/Dead State State")]
public class D_DeadState : ScriptableObject
{
    public GameObject _deathChunckParticle;
    public GameObject _deathBloodParticle;
}
