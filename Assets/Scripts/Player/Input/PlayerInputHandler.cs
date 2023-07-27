using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Camera _cam;

    public Vector2 _rawMovementInput { get; private set; }
    public Vector2 _rawDashDirectionInput { get; private set; }
    public Vector2Int _dashDirectionInput { get; private set; }
    public int _normalizedInputX { get; private set; }
    public int _normalizedInputY { get; private set; }
    public bool _jumpInput { get; private set; }
    public bool _holdJumpInputStop { get; private set; }
    public bool _grabInput { get; private set; }
    public bool _dashInput { get; private set; }
    public bool _dashInputStop { get; private set; }

    public bool[] _attackInputs { get; private set; }


    [SerializeField]
    private float _inputHoldTime = 0.2f;

    private float _jumpInputStartTime;
    private float _dashInputStartTime;


    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        int count = Enum.GetValues(typeof(CombatInputs)).Length;
        _attackInputs = new bool[count];
        _cam = Camera.main;
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        _rawMovementInput = context.ReadValue<Vector2>();

        _normalizedInputX = Mathf.RoundToInt(_rawMovementInput.x);
        _normalizedInputY = Mathf.RoundToInt(_rawMovementInput.y);

        if(Mathf.Abs(_rawMovementInput.y) > 0.5f) 
        {
            _normalizedInputY = (int)(_rawMovementInput * Vector2.up).normalized.y;
        }
        else
        {
            _normalizedInputY = 0;
        }
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            _jumpInput = true;
            _holdJumpInputStop = false;
            _jumpInputStartTime = Time.time;
        }

        if(context.canceled)
        {
            _holdJumpInputStop = true;
        }
    }

    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            _grabInput = true;
        }

        if(context.canceled)
        {
            _grabInput = false;
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            _dashInput = true;
            _dashInputStop = false;
            _dashInputStartTime = Time.time;
        }
        else if(context.canceled)
        {
            _dashInputStop = true;
        }
    }

    public void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        _rawDashDirectionInput = context.ReadValue<Vector2>();

        if(_playerInput.currentControlScheme == "Keyboard")
        {
            _rawDashDirectionInput = _cam.ScreenToWorldPoint((Vector3)_rawDashDirectionInput) - transform.position;
        }

        _dashDirectionInput = Vector2Int.RoundToInt(_rawDashDirectionInput.normalized);
    }

    public void OnPrimaryAttackInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            _attackInputs[(int)CombatInputs.primary] = true;
        }

        if(context.canceled)
        {
            _attackInputs[(int)CombatInputs.primary] = false;
        }
    }

    public void OnSecondaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _attackInputs[(int)CombatInputs.secondary] = true;
        }

        if (context.canceled)
        {
            _attackInputs[(int)CombatInputs.secondary] = false;
        }
    }

    public void UseJumpInput() => _jumpInput = false;
    public void UseDashInput() => _dashInput = false;
    private void CheckJumpInputHoldTime()
    {
        if(Time.time >= _jumpInputStartTime + _inputHoldTime)
        {
            _jumpInput = false;
        }
    }
    private void CheckDashInputHoldTime()
    {
        if(Time.time >= _dashInputStartTime + _inputHoldTime)
        {
            _dashInput = false;
        }
    }
}

public enum CombatInputs
{
    primary,
    secondary
}
