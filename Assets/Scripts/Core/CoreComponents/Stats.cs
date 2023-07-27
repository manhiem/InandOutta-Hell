using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : CoreComponent
{
    [SerializeField]
    private float _maxHealth;
    private float _curHealth;

    protected override void Awake()
    {
        base.Awake();
        _curHealth = _maxHealth;
    }

    public void DecreaseHealth(float health)
    {
        _curHealth -= health;

        if(_curHealth <= 0 )
        {
            _curHealth = 0;
            Debug.Log("Health is 0");
        }
    }

    public void IncreaseHealth(float health)
    {
        _curHealth += Mathf.Clamp(_curHealth+health, 0, _maxHealth);
    }
}
