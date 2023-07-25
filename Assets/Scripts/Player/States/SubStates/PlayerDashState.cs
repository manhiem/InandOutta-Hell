using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    public bool _canDash { get; private set; }

    private bool _isHolding;
    private bool _dashInputStop;
    private Vector2 _dashDirection;
    private Vector2 _lsatAfterImagePosition;

    private float _lastDashTime;

    private Vector2 _dashDirectionInput;
    public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        _canDash = false;
        player.inputHandler.UseDashInput();

        _isHolding = true;
        _dashDirection = Vector2.right * player._facingDirection;

        Time.timeScale = playerData._holdTimeScale;
        _startTime = Time.unscaledTime;

        player.dashDirectionIndicator.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        base.Exit();
        if(player._curVelocity.y > 0)
        {
            player.SetVelocityY(player._curVelocity.y * playerData._dashEndYMultiplier);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(!_isExitingState)
        {

            player._anim.SetFloat("xVelocity", player._curVelocity.y);
            player._anim.SetFloat("yVelocity", Mathf.Abs(player._curVelocity.x));

            if(_isHolding)
            {
                _dashDirectionInput = player.inputHandler._dashDirectionInput;
                _dashInputStop = player.inputHandler._dashInputStop;

                if(_dashDirectionInput!= Vector2.zero)
                {
                    _dashDirection = _dashDirectionInput;
                    _dashDirection.Normalize();
                }

                float angle = Vector2.SignedAngle(Vector2.right, _dashDirection);
                player.dashDirectionIndicator.rotation = Quaternion.Euler(0f, 0f, angle - 45f);

                if(_dashInputStop || Time.unscaledTime >= _startTime + playerData._maxHoldTime)
                {
                    _isHolding = false;
                    Time.timeScale = 1f;
                    _startTime = Time.time;
                    player.CheckIfShouldFlip(Mathf.RoundToInt(_dashDirection.x));
                    player.rb.drag = playerData._drag;
                    player.SetVelocity(playerData._dashVelocity, _dashDirection);
                    player.dashDirectionIndicator.gameObject.SetActive(false);
                    PlaceAfterImage();
                }
            }
            else
            {
                player.SetVelocity(playerData._dashVelocity, _dashDirection);
                CheckIfShouldPlaceAfterImage();
                if (Time.time >= _startTime + playerData._dashTime)
                {
                    player.rb.drag = 0f;
                    _isAbilityDone = true;
                    _lastDashTime = Time.time;
                }
            }
        }
    }

    private void CheckIfShouldPlaceAfterImage()
    {
        if(Vector2.Distance(player.transform.position, _lsatAfterImagePosition) >= playerData._distanceBetweenAfterImages)
        {
            PlaceAfterImage();
        } 
    }
    private void PlaceAfterImage()
    {
        AfterImagePool.Instance.GetFromPool();
        _lsatAfterImagePosition = player.transform.position;
    }

    public bool CheckIfCanDash()
    {
        return _canDash && (Time.time >= _lastDashTime + playerData._dashCoolDown);
    }

    public void ResetCanDash() => _canDash = true;

}
