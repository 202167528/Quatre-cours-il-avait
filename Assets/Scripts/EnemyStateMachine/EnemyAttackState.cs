using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyStateMachine currentContext, EnemyStateFactory playerStateFactory) : base
        (currentContext, playerStateFactory) {}

    public override void EnterState()
    {
        Ctx.Timer = Ctx.AttackDelay;
        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementZ = 0;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        Ctx.SetTimer();
        HandleAttack();
    }

    public override void ExitState()
    {
        Ctx.Timer = 0;
        Ctx.Animator.SetBool(Ctx.IsAttackingHash, false);
    }

    public override void CheckSwitchStates()
    {
        var distance = Vector3.Distance(Ctx.AIData.currentTarget.position, Ctx.CharacterController.transform.position);

        if (distance > Ctx.AttackDistance)
        {
            SwitchState(Factory.Run());
        }
    }

    public override void InitializeSubState() {}

    private void HandleAttack()
    {
        if (Ctx.Timer < Ctx.AttackDelay)
            return;
        
        Ctx.Animator.SetInteger(Ctx.AttackIndexHash, Ctx.GetRandomIndex());
        Ctx.Animator.SetBool(Ctx.IsAttackingHash, true);
        Ctx.Animator.SetBool(Ctx.IsWalkingHash, false);
        Ctx.Animator.SetBool(Ctx.IsRunningHash, false);
        Ctx.Timer = 0;
    }
}
