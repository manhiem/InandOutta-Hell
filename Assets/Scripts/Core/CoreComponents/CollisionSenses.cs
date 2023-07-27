using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionSenses : CoreComponent
{
    public Transform GroundCheck { get => groundCheck; set => groundCheck = value; }
    public Transform WallCheck { get => wallCheck; set => wallCheck = value; }
    public Transform LedgeCheck { get => ledgeCheck; set => ledgeCheck = value; }

    public float GroundCheckRadius { get => _groundCheckRadius; set => _groundCheckRadius = value; }
    public float WallCheckDistance { get => _wallCheckDistance; set => _wallCheckDistance = value; }
    public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }


    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform wallCheck;
    [SerializeField]
    private Transform ledgeCheck;
    [SerializeField]
    private float _groundCheckRadius;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private float _wallCheckDistance;


    public bool Grounded
    {
        get => Physics2D.OverlapCircle(groundCheck.position, _groundCheckRadius, whatIsGround);
    }

    public bool Ledge
    {
        get =>  Physics2D.Raycast(ledgeCheck.position, Vector2.right * _core._movement._facingDirection, _wallCheckDistance, whatIsGround);
    }

    public bool WallFront
    {
        get =>  Physics2D.Raycast(wallCheck.position, Vector2.right * _core._movement._facingDirection, _wallCheckDistance, whatIsGround);
    }

    public bool WallBack
    {
        get =>  Physics2D.Raycast(wallCheck.position, Vector2.right * -_core._movement._facingDirection, _wallCheckDistance, whatIsGround);
    }
}
