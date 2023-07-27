using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D rb { get; private set; }
    public int _facingDirection {  get; private set; }
    public Vector2 _curVelocity {  get; private set; }
    private Vector2 _velocityWorkspace;
    #region Set Functions
    protected override void Awake()
    {
        base.Awake();
        _facingDirection = 1;
        rb = GetComponentInParent<Rigidbody2D>();
    }

    public void LogicUpdate()
    {
        _curVelocity = rb.velocity;
    }

    public void SetVelocityX(float velocity)
    {
        _velocityWorkspace.Set(velocity, _curVelocity.y);
        rb.velocity = _velocityWorkspace;
        _curVelocity = _velocityWorkspace;
    }

    public void SetVelocityY(float velocity)
    {
        _velocityWorkspace.Set(_curVelocity.x, velocity);
        rb.velocity = _velocityWorkspace;
        _curVelocity = _velocityWorkspace;
    }

    public void SetVelocity(float velocity, Vector2 angle, int _direction)
    {
        angle.Normalize();
        _velocityWorkspace.Set(angle.x * velocity * _direction, angle.y * velocity);
        rb.velocity = _velocityWorkspace;
        _curVelocity = _velocityWorkspace;
    }

    public void SetVelocity0()
    {
        rb.velocity = Vector2.zero;
        _curVelocity = Vector2.zero;
    }

    public void SetVelocity(float velocity, Vector2 direction)
    {
        _velocityWorkspace = velocity * direction;
        rb.velocity = _velocityWorkspace;
        _curVelocity = _velocityWorkspace;
    }

    private void Flip()
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
