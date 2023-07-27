using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState 
{
    protected Core _core; 


    protected Player player;
    protected PlayerStateMachine stateMachine;
    protected PlayerData playerData;
    protected float _startTime;
    protected bool _isAnimationFinished;
    protected bool _isExitingState;

    private string _animBoolName;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.playerData = playerData;
        _animBoolName = animBoolName;
        _core = player._core;
    }

    public virtual void Enter() 
    {
        DoChecks();
        _startTime = Time.time;
        player._anim.SetBool(_animBoolName, true);
        Debug.Log(_animBoolName);
        _isAnimationFinished = false;
        _isExitingState = false;
    }
    public virtual void Exit() 
    {
        player._anim.SetBool(_animBoolName, false);
        _isExitingState = true;
    }
    public virtual void LogicUpdate() { }
    public virtual void PhysicsUpdate() 
    {
        DoChecks();
    }
    public virtual void DoChecks() { }

    public virtual void AnimationTrigger() { }

    public virtual void AnimationFinishTrigger() => _isAnimationFinished = true;
}
