using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyBaseState, IRootState
{
    public EnemyPatrolState(EnemyStateMachine currentContext, EnemyStateFactory playerStateFactory) : base
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
        Ctx.MovementDirectionSolver.PatrolPointsPosition = Ctx.PatrolPoints[Ctx.CurrentPoint].transform.position;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState() {}

    public override void CheckSwitchStates()
    {
        // Si l'agent voit le joueur, switch au Chase state
        if (Ctx.AIData.currentTarget != null)
        {
            SwitchState(Factory.Chase());
        } else if (Vector3.Distance(Ctx.CharacterController.transform.position, 
                       Ctx.PatrolPoints[Ctx.CurrentPoint].transform.position) < 0.2f)
        {
            SwitchState(Factory.Wait());
        }
    }

    public override void InitializeSubState()
    {
        SetSubState(Factory.Walk());
    }
    
    private void HandleAnimation()
    {
        Ctx.Animator.SetBool(Ctx.IsWalkingHash,true);
        Ctx.Animator.SetBool(Ctx.IsRunningHash,false);

        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementZ = 0;
    }
}
