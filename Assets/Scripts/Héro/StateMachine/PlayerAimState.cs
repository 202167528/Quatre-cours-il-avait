using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimState : PlayerBaseState, IRootState
{
    private float layerWeight;
    
    public PlayerAimState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        IsRootState = true;
    }
    
    public override void EnterState()
    {
        layerWeight = 0.0f;
        InitializeSubState();
        HandleGravity();
        HandleAim();
        HandleAnimation();
    }

    public override void UpdateState()
    {
        HandleLayerWeight();
        Ctx.Animator.SetLayerWeight(1, layerWeight);
        HandleGravity();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Ctx.Animator.SetLayerWeight(1, 0.0f);
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.CharacterController.isGrounded)
        {
            SwitchState(Factory.Fall());
        } else if (!Ctx.IsAimPressed)
        {
            SwitchState(Factory.Grounded());
        }
    }

    public override void InitializeSubState()
    {
        if (!Ctx.IsMovementPressed)
        {
            SetSubState(Factory.Idle());
        } else if (Ctx.IsMovementPressed)
        {
            SetSubState(Factory.Walk());
        }
    }

    public void HandleAim()
    {
        var targets = Physics.OverlapSphere(Ctx.CharacterController.transform.position, Ctx.TargetDetectionRadius, Ctx.TargetLayerMask);

        if (targets.Length > 0)
        {
            Ctx.TargetPosition = targets[0].transform.position;
        }
        else
        {
            Ctx.TargetPosition = Ctx.CharacterController.transform.forward;
        }
    }

    public void HandleGravity()
    {
        Ctx.CurrentMovementY = Ctx.Gravity;
        Ctx.AppliedMovementY = Ctx.Gravity;
    }

    private void HandleAnimation()
    {
        if (Ctx.ItemManager.equippedWeapon != null)
        {
            Debug.Log(Ctx.ItemManager.equippedWeapon != null);
            Ctx.Animator.SetBool(Ctx.IsAimingWithWeaponHash, true);
        }
    }

    private void HandleLayerWeight()
    {
        layerWeight += 0.01f;
    }
}
