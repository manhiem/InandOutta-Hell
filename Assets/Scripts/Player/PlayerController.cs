using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _anim;

    private float _movementInputDirection;
    private bool _isFacingRight = true;
    private bool _isWalking;
    private bool _isGrounded;
    private bool _canNormalJump;
    private bool _canWallJump;
    private bool _isTouchingWall;
    private bool _isWallSliding;
    private int _amountOfJumpsLeft;
    private int _facingDirection = 1;
    private float _jumpTimer;
    private bool _isAttemptingToJump;
    private bool _checkJumpMultiplier;
    private bool _canMove;
    private bool _canFlip;
    private float _turnTimer;
    private bool _isDashing;
    private float _dashTimeLeft;
    private float _lastImageXPos;
    private float _lastDash = -100;


    [Header("Movement Parameters")]
    public float _moveSpeed = 10.0f;

    [Header("jump Parameters")]
    public Transform groundCheck;
    public Transform wallCheck;
    public float _jumpForce = 16f;
    public float _groundCheckRadius;
    public LayerMask whatIsGround;
    public int _amountOfJumps = 1;
    public float _wallCheckDistance;
    public float _wallSlideSpeed;
    public float _movementForceInAir;
    public float _airDragMultiplier = 0.95f;
    public float _variableJumpHeightMultiplier = 0.5f;
    public Vector2 _wallHopDirection;
    public Vector2 _wallJumpDirection;
    public float _wallHopForce;
    public float _wallJumpForce;
    public float _jumpTimerSet = 0.15f;
    public float _turnTimerSet = 0.1f;
    public float _dashTime;
    public float _dashSpeed;
    public float _distanceBetweenImages;
    public float _dashCoolDown;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _amountOfJumpsLeft = _amountOfJumps;

        _wallHopDirection.Normalize();
        _wallJumpDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckIfWallSliding();
        CheckJump();
        CheckDash();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    private void UpdateAnimations()
    {
        _anim.SetBool("isWalking", _isWalking);
        _anim.SetBool("isGrounded", _isGrounded);
        _anim.SetFloat("YVelocity", _rb.velocity.y);
        _anim.SetBool("isWallSliding", _isWallSliding);
    }

    private void CheckInput()
    {
        _movementInputDirection = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown("Jump"))
        {
            // Checking if is on ground or check if player is on wall
            if(_isGrounded || (_amountOfJumpsLeft > 0 && _isTouchingWall))
            {
                NormalJump();
            }
            else
            {
                _jumpTimer = _jumpTimerSet;
                _isAttemptingToJump = true;
            }
        }

        // Check for wall jump. Provide a time gap for pressing the space key after A or D
        if(Input.GetButtonDown("Horizontal") && _isTouchingWall)
        {
            if(!_isGrounded && _movementInputDirection != _facingDirection)
            {
                _canMove = false;
                _canFlip = false;

                _turnTimer = _turnTimerSet;
            }
        }

        if(!_canMove)
        {
            _turnTimer -= Time.deltaTime;

            if(_turnTimer <=0)
            {
                _canMove = true;
                _canFlip = true;
            }
        }

        if(_checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            _checkJumpMultiplier = false;
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y * _variableJumpHeightMultiplier);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(Time.time >= _lastDash + _dashCoolDown)
            {
                AttemptToDash();
            }
        }
    }

    private void AttemptToDash()
    {
        _isDashing = true;
        _dashTimeLeft = _dashTime;
        _lastDash = Time.time;

        AfterImagePool.Instance.GetFromPool();
        _lastImageXPos = transform.position.x;
    }

    private void CheckDash()
    {
        if(_isDashing)
        {
            if(_dashTimeLeft > 0)
            {
                _canMove = false;
                _canFlip = false;
                _rb.velocity = new Vector2(_dashSpeed * _facingDirection, 0);
                _dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - _lastImageXPos) > _distanceBetweenImages)
                {
                    AfterImagePool.Instance.GetFromPool();
                    _lastImageXPos = transform.position.x;
                }
            }

            if(_dashTimeLeft <= 0 || _isTouchingWall)
            {
                _isDashing = false;
                _canFlip = true;
                _canMove = true;
            }
        }
    }

    private void CheckMovementDirection()
    {
        if(_isFacingRight && _movementInputDirection < 0)
        {
            Flip();
        }
        else if (!_isFacingRight && _movementInputDirection > 0)
        {
            Flip();
        }

        if(Mathf.Abs(_rb.velocity.x) >= 0.01f)
        {
            _isWalking = true;
        }
        else
        {
            _isWalking = false;
        }
    }

    private void ApplyMovement()
    {
        if (!_isGrounded && !_isWallSliding && _movementInputDirection == 0f)
        {
            _rb.velocity = new Vector2(_rb.velocity.x * _airDragMultiplier, _rb.velocity.y);
        }
        else if(_canMove)
        {
            _rb.velocity = new Vector2(_moveSpeed * _movementInputDirection, _rb.velocity.y);
        }


        if(_isWallSliding)
        {
            if(_rb.velocity.y < -_wallSlideSpeed)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, -_wallSlideSpeed);
            }
        }
    }

    private void CheckJump()
    {
        if(_jumpTimer > 0)
        {
            // Wall Jump
            if(!_isGrounded && _isTouchingWall && _movementInputDirection != 0 && _movementInputDirection != _facingDirection)
            {
                WallJumpAndHop();
            }
            else if(_isGrounded)
            {
                NormalJump(); 
            }
        }

        if(_isAttemptingToJump)
        {
            _jumpTimer -= Time.deltaTime;
        }
    }

    private void NormalJump()
    {
        if (_canNormalJump)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
            _amountOfJumpsLeft--;
            _jumpTimer = 0;
            _isAttemptingToJump = false;
            _checkJumpMultiplier = true;
        }
    }

    private void WallJumpAndHop()
    {
        if (_canWallJump)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0.0f);
            _isWallSliding = false;
            _amountOfJumpsLeft = _amountOfJumps;
            _amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(_wallJumpForce * _wallJumpDirection.x * _movementInputDirection, _wallJumpForce * _wallJumpDirection.y);
            _rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            _jumpTimer = 0;
            _isAttemptingToJump = false;
            _checkJumpMultiplier = true;
            _turnTimer = 0;
            _canMove = true;
            _canFlip = true;
        }
        else if (_isWallSliding && _movementInputDirection == 0 && _canNormalJump)
        {
            _isWallSliding = false;
            _amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(_wallHopForce * _wallHopDirection.x * -_facingDirection, _wallHopForce * _wallHopDirection.x);
            _rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
    }

    private void CheckSurroundings()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, _groundCheckRadius, whatIsGround);

        _isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, _wallCheckDistance, whatIsGround);
    }

    private void CheckIfWallSliding()
    {
        if(_isTouchingWall && _movementInputDirection == _facingDirection && _rb.velocity.y < 0)
        {
            _isWallSliding = true;
        }
        else
        {
            _isWallSliding = false;
        }
    }

    private void Flip()
    {
        if(!_isWallSliding && _canFlip)
        {
            _facingDirection *= -1;
            _isFacingRight = !_isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    public void DisableFlip()
    {
        _canFlip = false;
    }

    public void EnableFlip()
    {
        _canFlip = true;
    }

    private void CheckIfCanJump()
    {
        if(_isGrounded && _rb.velocity.y <= 0.01f)
        {
            _amountOfJumpsLeft = _amountOfJumps;
        }

        if(_isTouchingWall)
        {
            _canWallJump = true;
        }
        
        if(_amountOfJumpsLeft <= 0)
        {
            _canNormalJump = false;
        }
        else
        {
            _canNormalJump = true;
        }
    }

    public int GetFacingDirection()
    {
        return _facingDirection;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, _groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + _wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}
