using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 _rawMovementInput { get; private set; }
    public int _normalizedInputX { get; private set; }
    public int _normalizedInputY { get; private set; }
    public bool _jumpInput { get; private set; }
    public bool _holdJumpInputStop { get; private set; }
    public bool _grabInput { get; private set; }


    [SerializeField]
    private float _inputHoldTime = 0.2f;

    private float _jumpInputStartTime;

    private void Update()
    {
        CheckJumpInputHoldTime();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        _rawMovementInput = context.ReadValue<Vector2>();

        if(Mathf.Abs(_rawMovementInput.x) > 0.5f)
        {
            _normalizedInputX = (int)(_rawMovementInput * Vector2.right).normalized.x;
        }
        else
        {
            _normalizedInputX = 0;
        }

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

    public void UseJumpInput() => _jumpInput = false;
    private void CheckJumpInputHoldTime()
    {
        if(Time.time >= _jumpInputStartTime + _inputHoldTime)
        {
            _jumpInput = false;
        }
    }
}
