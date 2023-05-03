using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState, IRootState
{
    private float distance;
    
    public EnemyChaseState(EnemyStateMachine currentContext, EnemyStateFactory playerStateFactory) : base
        (currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public void HandleGravity()
    {
        Ctx.AppliedMovementY = Physics.gravity.y;
    }
    
    public override void EnterState()
    {
        InitializeSubState();
        HandleGravity();
        HandleAnimation();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates()
    {
        if (Ctx.AIData.currentTarget == null)
        {
            SwitchState(Factory.Patrol());
        }
    }

    public override void InitializeSubState()
    {
        if (Ctx.AIData.currentTarget != null)
        {
            distance = Vector3.Distance(Ctx.AIData.currentTarget.position, Ctx.CharacterController.transform.position);
            
            if (distance <= Ctx.AttackDistance)
            {
                SetSubState(Factory.Attack());
            }
            else
            {
                SetSubState(Factory.Run());
            }
        }
    }
    
    private void HandleAnimation()
    {
        if (distance <= Ctx.AttackDistance)
        {
            Ctx.Animator.SetBool(Ctx.IsWalkingHash, false);
            Ctx.Animator.SetBool(Ctx.IsRunningHash, false);
            Ctx.Animator.SetFloat(Ctx.AttackIndexHash, Ctx.GetRandomIndex());
            Ctx.Animator.SetBool(Ctx.IsAttackingHash, true);
        }
        else
        {
            Ctx.Animator.SetBool(Ctx.IsWalkingHash, true);
            Ctx.Animator.SetBool(Ctx.IsRunningHash, true);
            Ctx.Animator.SetBool(Ctx.IsAttackingHash, false);
        }
        
        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementZ = 0;
    }
}
