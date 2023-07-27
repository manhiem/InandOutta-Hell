using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollisionSenses : CoreComponent
{
    public Transform GroundCheck 
    {
        get => GenericNotImplementedError<Transform>.TryGet(groundCheck, _core.transform.parent.name);
        private set => groundCheck = value; 
    }
    public Transform WallCheck 
    {
        get => GenericNotImplementedError<Transform>.TryGet(wallCheck, _core.transform.parent.name);
        private set => wallCheck = value; 
    }
    public Transform LedgeCheckHorizontal 
    {
        get => GenericNotImplementedError<Transform>.TryGet(ledgeCheckHorizontal, _core.transform.parent.name);
        private set => ledgeCheckHorizontal = value; 
    }

    public Transform LedgeCheckVertical 
    {
        get => GenericNotImplementedError<Transform>.TryGet(ledgeCheckVertical, _core.transform.parent.name);
        private set => ledgeCheckVertical = value; 
    }

    public float GroundCheckRadius { get => _groundCheckRadius; set => _groundCheckRadius = value; }
    public float WallCheckDistance { get => _wallCheckDistance; set => _wallCheckDistance = value; }
    public LayerMask WhatIsGround { get => whatIsGround; set => whatIsGround = value; }


    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private Transform wallCheck;
    [SerializeField]
    private Transform ledgeCheckHorizontal;
    [SerializeField]
    private Transform ledgeCheckVertical;
    [SerializeField]
    private float _groundCheckRadius;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private float _wallCheckDistance;


    public bool Grounded
    {
        get => Physics2D.OverlapCircle(GroundCheck.position, _groundCheckRadius, whatIsGround);
    }

    public bool LedgeHorizontal
    {
        get =>  Physics2D.Raycast(LedgeCheckHorizontal.position, Vector2.right * _core._movement._facingDirection, _wallCheckDistance, whatIsGround);
    }
    
    public bool LedgeVertical
    {
        get =>  Physics2D.Raycast(LedgeCheckVertical.position, Vector2.down, _wallCheckDistance, whatIsGround);
    }

    public bool WallFront
    {
        get =>  Physics2D.Raycast(WallCheck.position, Vector2.right * _core._movement._facingDirection, _wallCheckDistance, whatIsGround);
    }

    public bool WallBack
    {
        get =>  Physics2D.Raycast(WallCheck.position, Vector2.right * -_core._movement._facingDirection, _wallCheckDistance, whatIsGround);
    }
}
