using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/ Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")]
    public float _moveSpeed = 10f;

    [Header("Jump State")]
    public float _jumpVelocity = 15f;
    public int _amountOfJumps = 1;

    [Header("InAir State")]
    public float _coyoteTime = 0.2f;
    public float _jumpHeightMultiplier = 0.5f;

    [Header("Check Variables")]
    public float _groundCheckRadius = 0.3f;
    public LayerMask whatIsGround;
    public float _wallCheckDistance = 0.5f;

    [Header("Wall Slide")]
    public float _wallSlideVelocity = 3f;

    [Header("Wall Climb State")]
    public float _wallClimbVelocity = 3f;

    [Header("Wall Jump State")]
    public float _wallJumpVelocity = 20f;
    public float _wallJumpTime = 0.4f;
    public Vector2 _wallJumpAngle = new Vector2(1, 2);

    [Header("Ledge Climb State")]
    public Vector2 _startOffset;
    public Vector2 _stopOffset;

    [Header("Dash State")]
    public float _dashCoolDown = 0.5f;
    public float _maxHoldTime = 1f;
    public float _holdTimeScale = 0.25f;
    public float _dashTime = 0.2f;
    public float _dashVelocity = 30f;
    public float _drag = 10f;
    public float _dashEndYMultiplier = 0.2f;
    public float _distanceBetweenAfterImages = 0.5f;

}
