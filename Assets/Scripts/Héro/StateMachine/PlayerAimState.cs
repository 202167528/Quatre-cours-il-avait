using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimState : PlayerBaseState
{
    public PlayerAimState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {}
    
    public override void EnterState()
    {
        Ctx.Animator.SetBool(Ctx.IsWalkingHash, false);
        HandleAim();
        Debug.Log(Ctx.Target);
    }

    public override void UpdateState()
    {
        HandleAnimation();
        Ctx.AppliedMovementX = Ctx.MoveMultiplier * Ctx.CurrentMovementInput.x;
        Ctx.AppliedMovementZ = Ctx.MoveMultiplier * Ctx.CurrentMovementInput.y;
        CheckSwitchStates();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsAimPressed)
        {
            SwitchState(Factory.Idle());
        } else if (Ctx.IsUsePressed)
        {
            SwitchState(Factory.Use());
        } else if (Ctx.IsInteractPressed)
        {
            SwitchState(Factory.Interact());
        }
    }

    public override void InitializeSubState() {}

    public void HandleAim()
    {
        var targets = Physics.OverlapSphere(Ctx.CharacterController.transform.position, Ctx.TargetDetectionRadius, Ctx.TargetLayerMask);

        if (targets.Length > 0)
        {
            Ctx.Target = targets[0].gameObject;
        }
        else
        {
            Ctx.Target = null;
        }
    }

    private void HandleAnimation()
    {
        if (Ctx.IsMovementPressed || (Ctx.IsRunPressed && Ctx.IsMovementPressed))
        {
            Ctx.Animator.SetBool(Ctx.IsWalkingHash, true);
        }
        else
        {
            Ctx.Animator.SetBool(Ctx.IsWalkingHash, false);
        }
    }
}
