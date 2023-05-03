using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkState : EnemyBaseState
{
    public EnemyWalkState(EnemyStateMachine currentContext, EnemyStateFactory playerStateFactory) : base
        (currentContext, playerStateFactory) {}

    public override void EnterState() {}

    public override void UpdateState()
    {
        HandleWalk();
    }

    public override void ExitState()
    {
        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementZ = 0;
    }

    public override void CheckSwitchStates()
    {
    }

    public override void InitializeSubState() {}

    private void HandleWalk()
    {
        var direction = Ctx.MovementDirectionSolver.GetDirectionToMove(Ctx.SteeringBehaviours, Ctx.AIData);

        Ctx.AppliedMovementX = direction.x * Ctx.MaxSpeed;
        Ctx.AppliedMovementZ = direction.z * Ctx.MaxSpeed;
    }
}
