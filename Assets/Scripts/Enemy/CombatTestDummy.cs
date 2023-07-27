using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTestDummy : MonoBehaviour, Idamagable
{
    [SerializeField]
    private GameObject _hitParticles;

    private Animator _anim;

    public void Damage(float amount)
    {
        Instantiate(_hitParticles, transform.position, Quaternion.Euler(0f, 0f,  Random.Range(0f, 360f)));
        _anim.SetTrigger("Damage");
        Destroy(gameObject);
        Debug.Log(amount + " damage taken");
    }

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
}
