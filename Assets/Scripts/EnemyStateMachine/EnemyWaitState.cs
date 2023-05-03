using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaitState : EnemyBaseState
{
    public EnemyWaitState(EnemyStateMachine currentContext, EnemyStateFactory playerStateFactory) : base
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
        
        if (Ctx.AIData.currentTarget == null)
        {
            HandlePatrolPoint();
        }
    }

    public override void UpdateState()
    {
        Ctx.SetTimer();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Ctx.Timer = 0.0f;
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.Timer >= Ctx.WaitTime)
        {
            SwitchState(Factory.Patrol());
        }
    }

    public override void InitializeSubState() {}

    private void HandlePatrolPoint()
    {
        Ctx.CurrentPoint = (Ctx.CurrentPoint + 1) % Ctx.PatrolPointsLenght;
        Ctx.MovementDirectionSolver.PatrolPointsPosition = Ctx.PatrolPoints[Ctx.CurrentPoint].transform.position;
    }
    
    private void HandleAnimation()
    {
        Ctx.Animator.SetBool(Ctx.IsWalkingHash,false);
        Ctx.Animator.SetBool(Ctx.IsRunningHash,false);

        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementZ = 0;
    }
}
