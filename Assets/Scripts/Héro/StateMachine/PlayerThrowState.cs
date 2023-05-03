using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerThrowState : PlayerBaseState
{
    public PlayerThrowState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base
        (currentContext, playerStateFactory) {}

    public override void EnterState()
    {
        // Joue l'animation de lancer
    }

    public override void UpdateState()
    {
        Ctx.AppliedMovementX = Ctx.MoveMultiplier * Ctx.CurrentMovementInput.x;
        Ctx.AppliedMovementZ = Ctx.MoveMultiplier * Ctx.CurrentMovementInput.y;
        CheckSwitchStates();
        HandleTrail();
    }

    public override void ExitState()
    {
        HandleThrow();
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsThrowPressed)
        {
            SwitchState(Factory.Idle());
        }
    }

    public override void InitializeSubState(){}

    private void HandleTrail()
    {
        
    }

    private void HandleThrow()
    {
        var weapon = Ctx.ItemManager.equippedWeapon.GameObject();
        var rb = weapon.GetComponent<Rigidbody>();
        weapon.transform.parent = null;

        var force = new Vector3(0, Ctx.MaxThrowUpwardDistance, 0) + new Vector3(0, 0, Ctx.MaxThrowDistance);
        rb.AddForce(force, ForceMode.Impulse);
    }
}
