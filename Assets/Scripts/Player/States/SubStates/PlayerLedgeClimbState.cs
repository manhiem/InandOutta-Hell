using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerLedgeClimbState : PlayerState
{
    private Vector2 _detectedPos;
    private Vector2 _cornerPos;
    private Vector2 _startPos;
    private Vector2 _stopPos;
    private Vector2 _velocityWorkspace;

    private bool _isHanging;
    private bool _isClimbing;

    private int _xInput;
    private int _yInput;
    private bool _jumpInput;
    public PlayerLedgeClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        player._anim.SetBool("Climb Ledge", false);
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        _isHanging = true;
    }

    public override void Enter()
    {
        base.Enter();
        _core._movement.SetVelocity0();
        player.transform.position = _detectedPos;
        _cornerPos = DetermineCornerPosition();
        _startPos.Set(_cornerPos.x - (_core._movement._facingDirection * playerData._startOffset.x), _cornerPos.y - playerData._startOffset.y);
        _stopPos.Set(_cornerPos.x + (_core._movement._facingDirection * playerData._stopOffset.x), _cornerPos.y + playerData._stopOffset.y);

        player.transform.position = _startPos;
    }

    public override void Exit()
    {
        base.Exit();

        _isHanging = false;

        if(_isClimbing)
        {
            player.transform.position = _stopPos;
            _isClimbing = false;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(_isAnimationFinished)
        {
            stateMachine.ChangeState(player.idleState);
        }
        else
        {
            _xInput = player.inputHandler._normalizedInputX;
            _yInput = player.inputHandler._normalizedInputY;
            _jumpInput = player.inputHandler._jumpInput;

            _core._movement.SetVelocity0();
            player.transform.position = _startPos;

            if(_xInput == _core._movement._facingDirection && _isHanging && !_isClimbing)
            {
                _isClimbing = true;
                player._anim.SetBool("Climb Ledge", true);
            }
            else if(_yInput == -1f && _isHanging && !_isClimbing)
            {
                stateMachine.ChangeState(player.airState);
            }
            else if(_jumpInput && !_isClimbing)
            {
                player.wallJumpState.DeterminewallJumpDirection(true);
                stateMachine.ChangeState(player.wallJumpState);
            }
        }


    }

    public void SetDetectedPosition(Vector2 pos)
    {
        _detectedPos = pos;
    }

    private Vector2 DetermineCornerPosition()
    {
        RaycastHit2D xHit = Physics2D.Raycast(_core._collisionSenses.WallCheck.position, Vector2.right * _core._movement._facingDirection, _core._collisionSenses.WallCheckDistance, _core._collisionSenses.WhatIsGround);
        float xDistance = xHit.distance;
        _velocityWorkspace.Set(xDistance * _core._movement._facingDirection, 0);

        RaycastHit2D yHit = Physics2D.Raycast(_core._collisionSenses.LedgeCheck.position + (Vector3)_velocityWorkspace, Vector2.down, _core._collisionSenses.LedgeCheck.position.y - _core._collisionSenses.WallCheck.position.y, _core._collisionSenses.WhatIsGround);
        float yDistance = yHit.distance;

        _velocityWorkspace.Set(_core._collisionSenses.WallCheck.position.x + (xDistance * _core._movement._facingDirection), _core._collisionSenses.LedgeCheck.position.y - yDistance);
        return _velocityWorkspace;
    }
}
