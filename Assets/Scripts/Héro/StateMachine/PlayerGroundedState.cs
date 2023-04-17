using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState, IRootState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public void HandleGravity()
    {
        Ctx.CurrentMovementY = Ctx.Gravity;
        Ctx.AppliedMovementY = Ctx.Gravity;
    }

    public override void EnterState()
    {
        InitializeSubState();
        HandleGravity();
        HandleAnimation();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override  void ExitState(){}

    public override void CheckSwitchStates()
    {
        // Si le joueur est sur le sol et jump est pesé, switch au jump state
        if (Ctx.IsJumpPressed && !Ctx.RequireNewJumpPress)
        {
            SwitchState(Factory.Jump());
        } else if (Ctx.IsAimPressed)
        {
            SwitchState(Factory.Aim());
        }else if (!Ctx.CharacterController.isGrounded)
        {
            SwitchState(Factory.Fall());
        }
    }

    public override void InitializeSubState()
    {
        if (!Ctx.IsMovementPressed && !Ctx.IsRunPressed)
        {
            SetSubState(Factory.Idle());
        } else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed)
        {
            SetSubState(Factory.Walk());
        }
        else
        {
            SetSubState(Factory.Run());
        }
    }

    private void HandleAnimation()
    {
        if (Ctx.IsInteractPressed)
        {
            Ctx.Animator.SetBool(Ctx.IsInteractingHash, true);
        } else if (Ctx.IsMovementPressed || Ctx.IsRunPressed)
        {
            Ctx.Animator.SetBool(Ctx.IsWalkingHash, true);
        }
        else
        {
            Ctx.Animator.SetBool(Ctx.IsWalkingHash,false);
        }
        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementZ = 0;
    }
}
