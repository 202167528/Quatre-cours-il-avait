using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(currentContext, playerStateFactory) {}

    public override void EnterState()
    {
        Ctx.Animator.SetBool(Ctx.IsWalkingHash, false);
        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementZ = 0;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override  void ExitState(){}

    public override void CheckSwitchStates()
    {
        if (Ctx.IsInteractPressed && Ctx.ItemManager.equippedWeapon == null && Ctx.ItemManager.equippedPotion == null)
        {
            SwitchState(Factory.Interact());
        } else if (Ctx.IsUsePressed)
        {
            SwitchState(Factory.Use());
        }else if (Ctx.IsMovementPressed && Ctx.IsRunPressed)
        {
            SwitchState(Factory.Run());
        } else if (Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Walk());
        }
    }

    public override void InitializeSubState(){}

    private IEnumerator WaitForSeconds()
    {
        yield return new WaitForSeconds(10.0f);
    }
}
