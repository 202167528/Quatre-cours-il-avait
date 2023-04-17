using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base
        (currentContext, playerStateFactory) {}

    public override void EnterState()
    {
        Ctx.Animator.SetBool(Ctx.IsWalkingHash, true);
    }

    public override void UpdateState()
    {
        Ctx.AppliedMovementX = Ctx.CurrentMovementInput.x * Ctx.RunMultiplier;
        Ctx.AppliedMovementZ = Ctx.CurrentMovementInput.y * Ctx.RunMultiplier;
        CheckSwitchStates();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.IsInteractPressed && Ctx.ItemManager.equippedWeapon == null && Ctx.ItemManager.equippedPotion == null)
        {
            SwitchState(Factory.Interact());
        } else if (Ctx.IsUsePressed)
        {
            SwitchState(Factory.Use());
        }else if (!Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Idle());
        } else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed)
        {
            SwitchState(Factory.Walk());
        }
    }

    public override void InitializeSubState(){}
}
