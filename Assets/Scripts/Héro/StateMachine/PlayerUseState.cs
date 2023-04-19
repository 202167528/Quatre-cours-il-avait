using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseState : PlayerBaseState
{
    public PlayerUseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base
        (currentContext, playerStateFactory) {}

    public override void EnterState()
    {
        Ctx.IsAttacking = true;
        HandleUse();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Ctx.IsAttacking = false;
        Ctx.Animator.SetBool(Ctx.IsUsingHash, false);
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsAttacking)
        {
            if (!Ctx.IsMovementPressed)
            {
                SwitchState(Factory.Idle());
            } 
        } else switch (Ctx.IsMovementPressed)
        {
            case true when !Ctx.IsRunPressed:
                SwitchState(Factory.Walk());
                break;
            case true when Ctx.IsRunPressed:
                SwitchState(Factory.Run());
                break;
        }
    }

    public override void InitializeSubState(){}

    private void HandleUse()
    {
        if (Ctx.ItemManager.equippedWeapon == null)
        {
            Ctx.IsAttacking = false;
            return;
        }
        
        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementZ = 0;
        
        Ctx.Animator.SetInteger(Ctx.AttackIndexHash, Ctx.ItemManager.equippedWeapon.animation);
        Ctx.Animator.SetBool(Ctx.IsUsingHash, true);
    }
}
