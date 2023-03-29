using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base
        (currentContext, playerStateFactory)
    {
        
    }

    public override void EnterState()
    {
        Ctx.Animator.SetBool(Ctx.IsWalkingHash, true);
        Ctx.Animator.SetBool(Ctx.IsRunningHash, false);
    }

    public override void UpdateState()
    {
        Ctx.AppliedMovementX = 1.5f * Ctx.CurrentMovementInput.x;
        Ctx.AppliedMovementZ = 1.5f * Ctx.CurrentMovementInput.y;
        CheckSwitchStates();
    }

    public override  void ExitState(){}

    public override void CheckSwitchStates()
    {
        if (Ctx.IsInteractPressed)
        {
            SwitchState(Factory.Interact());
        } else if (!Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Idle());
        } else if (Ctx.IsMovementPressed && Ctx.IsRunPressed)
        {
            SwitchState(Factory.Run());
        }
    }

    public override void InitializeSubState(){}
}
