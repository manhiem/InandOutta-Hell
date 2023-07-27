using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D rb { get; private set; }
    public int _facingDirection {  get; private set; }
    public bool _canSetVelocity { get; set; }
    public Vector2 _curVelocity {  get; private set; }
    private Vector2 _velocityWorkspace;
    #region Set Functions
    protected override void Awake()
    {
        base.Awake();
        _facingDirection = 1;
        _canSetVelocity = true;
        rb = GetComponentInParent<Rigidbody2D>();
    }

    public override void LogicUpdate()
    {
        _curVelocity = rb.velocity;
    }

    public void SetVelocityX(float velocity)
    {
        _velocityWorkspace.Set(velocity, _curVelocity.y);
        SetFinalVelocity();
    }

    public void SetVelocityY(float velocity)
    {
        _velocityWorkspace.Set(_curVelocity.x, velocity);
        SetFinalVelocity();
    }

    public void SetVelocity(float velocity, Vector2 angle, int _direction)
    {
        angle.Normalize();
        _velocityWorkspace.Set(angle.x * velocity * _direction, angle.y * velocity);
        SetFinalVelocity();
    }

    public void SetVelocity0()
    {
        _velocityWorkspace = Vector2.zero;
        SetFinalVelocity();
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        _velocityWorkspace = velocity * direction;
        SetFinalVelocity();
    }

    private void SetFinalVelocity ()
    {
        if(_canSetVelocity)
        {
            rb.velocity = _velocityWorkspace;
            _curVelocity = _velocityWorkspace;
        }
    }

    public void Flip()
    {
        _facingDirection *= -1;
        rb.transform.Rotate(0.0f, 180f, 0.0f);
    }

    public void CheckIfShouldFlip(int xInput)
    {
        if (xInput != 0 && xInput != _facingDirection)
        {
            Flip();
        }
    }


    #endregion
}
