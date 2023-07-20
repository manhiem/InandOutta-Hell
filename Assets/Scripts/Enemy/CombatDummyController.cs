using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CombatDummyController : MonoBehaviour
{
    [SerializeField]
    private float _maxHeath, _knockbackSpeedX, _knockbackSpeedY, _knockbackDuration, _knockbackDeathSpeedX, _knockbackDeathSpeedY, _knockbackTorque;
    private float _curHealth, _knockbackStart;
    private int _playerFacingDirection;
    private bool _playerOnLeft, _knockback;

    [SerializeField]
    private GameObject _hitParticle;

    [SerializeField]
    private bool _applyKnockBack;

    [SerializeField]
    private PlayerController _PC;
    [SerializeField]
    private GameObject aliveGO, brokenTopGO, brokenBotGO;

    private Rigidbody2D rbAlive, rbBrokenTop, rbBrokenBot;
    private Animator _botAim;

    private void Start()
    {
        _curHealth = _maxHeath;

        _botAim = aliveGO.GetComponent<Animator>();
        rbAlive = aliveGO.GetComponent<Rigidbody2D>();
        rbBrokenBot = brokenBotGO.GetComponent<Rigidbody2D>();
        rbBrokenTop = brokenTopGO.GetComponent<Rigidbody2D>();

        aliveGO.SetActive(true);
        brokenTopGO.SetActive(false);
        brokenBotGO.SetActive(false);
    }

    private void Update()
    {
        CheckKnockBack();
    }

    public void Damage(AttackDetails damage)
    {
        _curHealth -= damage.damageAmount;
        _playerFacingDirection = _PC.GetFacingDirection();
        Instantiate(_hitParticle, aliveGO.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));

        if(_playerFacingDirection == 1)
        {
            _playerOnLeft = true;
        }
        else
        {
            _playerOnLeft = false;
        }

        _botAim.SetBool("PlayerOnLeft", _playerOnLeft);
        _botAim.SetTrigger("Damage");
        if(_applyKnockBack && _curHealth >= 0.0f)
        {
            // Apply knockback
            KnockBack();
        }

        if(_curHealth < 0.0f)
        {
            Die();
        }
    }

    private void KnockBack()
    {
        _knockback = true;
        _knockbackStart = Time.time;
        rbAlive.velocity = new Vector2(_knockbackSpeedX * _playerFacingDirection, _knockbackSpeedY);
    }

    private void CheckKnockBack()
    {
        if(Time.time >= _knockbackStart + _knockbackDuration && _knockback)
        {
            _knockback = false;
            rbAlive.velocity = new Vector2(0.0f, rbAlive.velocity.y);
        } 
    }

    private void Die()
    {
        aliveGO.SetActive(false);
        brokenTopGO.SetActive(true);
        brokenBotGO.SetActive(true);

        brokenBotGO.transform.position = aliveGO.transform.position;
        brokenTopGO.transform.position = aliveGO.transform.position;

        rbBrokenBot.velocity = new Vector2(_knockbackSpeedX * _playerFacingDirection, _knockbackSpeedY);
        rbBrokenTop.velocity = new Vector2(_knockbackDeathSpeedX * _playerFacingDirection, _knockbackDeathSpeedY);
        rbBrokenTop.AddTorque(_knockbackTorque * _playerFacingDirection, ForceMode2D.Impulse);
    }
}
