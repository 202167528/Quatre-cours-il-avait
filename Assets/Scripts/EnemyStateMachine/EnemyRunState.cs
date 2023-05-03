using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunState : EnemyBaseState
{
    public EnemyRunState(EnemyStateMachine currentContext, EnemyStateFactory playerStateFactory) : base
        (currentContext, playerStateFactory) {}

    public override void EnterState()
    {
        Ctx.Animator.SetBool(Ctx.IsWalkingHash, true);
        Ctx.Animator.SetBool(Ctx.IsRunningHash, true);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        HandleRun();
    }

    public override void ExitState()
    {
        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementZ = 0;
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.AIData.currentTarget != null)
        {
            var distance = Vector3.Distance(Ctx.AIData.currentTarget.position, Ctx.CharacterController.transform.position);

            if (distance <= Ctx.AttackDistance)
            {
                SwitchState(Factory.Attack());
            }
        }
    }

    public override void InitializeSubState() {}

    private void HandleRun()
    {
        var direction = Ctx.MovementDirectionSolver.GetDirectionToMove(Ctx.SteeringBehaviours, Ctx.AIData);

        Ctx.AppliedMovementX = direction.x * Ctx.MaxSpeed * Ctx.RunMultiplier;
        Ctx.AppliedMovementZ = direction.z * Ctx.MaxSpeed * Ctx.RunMultiplier;
    }
}
