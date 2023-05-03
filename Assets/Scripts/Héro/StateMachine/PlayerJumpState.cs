using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState, IRootState
{
    private float currentMovementX;
    private float currentMovementZ;
    private float currentMovement;
    
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void EnterState()
    {
        HandleJump();
    }

    public override void UpdateState()
    {
        if (Ctx.IsMovementPressed)
        {
            Ctx.AppliedMovementX = currentMovement != 0 ? Ctx.CurrentMovementInput.x * currentMovement : Ctx.CurrentMovementInput.x * Ctx.MoveMultiplier;
            Ctx.AppliedMovementZ = currentMovement != 0 ? Ctx.CurrentMovementInput.y * currentMovement : Ctx.CurrentMovementInput.y * Ctx.MoveMultiplier;
        }
        
        HandleGravity();
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Ctx.Animator.SetBool(Ctx.IsJumpingHash,false);
        if (Ctx.IsJumpPressed)
        {
            Ctx.RequireNewJumpPress = true;
        }
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.CharacterController.isGrounded)
        {
            SwitchState(Factory.Grounded());
        }
    }

    public override void InitializeSubState()
    {
    }

    private void HandleJump()
    {
        Ctx.Animator.SetBool(Ctx.IsJumpingHash,true);
        
        currentMovementX = Mathf.Abs(Ctx.AppliedMovementX);
        currentMovementZ = Mathf.Abs(Ctx.AppliedMovementZ);
        currentMovement = new Vector2(currentMovementX, currentMovementZ).magnitude;

        Ctx.CurrentMovementY = Ctx.InitialJumpVelocity;
        Ctx.AppliedMovementY = Ctx.InitialJumpVelocity;
    }

    public void HandleGravity()
    {
        var isFalling = Ctx.CurrentMovementY <= 0 || !Ctx.IsJumpPressed;
        
        if (isFalling)
        {
            var previousYVelocity = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY += (Ctx.Gravity * Ctx.FallMultiplier * Time.deltaTime);
            Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentMovementY) * 0.5f, -20.0f);
        }
        else
        {
            var previousYVelocity = Ctx.CurrentMovementY;
            Ctx.CurrentMovementY += (Ctx.Gravity * Time.deltaTime);
            Ctx.AppliedMovementY = Mathf.Max((previousYVelocity + Ctx.CurrentMovementY) * 0.5f, -20.0f);
        }
    }
}
