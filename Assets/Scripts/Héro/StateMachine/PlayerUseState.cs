using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseState : PlayerBaseState
{
    public PlayerUseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base
        (currentContext, playerStateFactory) {}

    public override void EnterState()
    {
        HandleAttack();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        //DamageEnemy();
    }

    public override void ExitState()
    {
        Ctx.Animator.ResetTrigger(Ctx.UseHash);
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsAttacking)
        {
            SwitchState(Factory.Idle());
        }
    }

    public override void InitializeSubState(){}

    private void HandleAttack()
    {
        if (Ctx.ItemManager.equippedWeapon == null)
        {
            return;
        }
        
        Ctx.AppliedMovementX = 0;
        Ctx.AppliedMovementZ = 0;
        
        Ctx.Animator.SetInteger(Ctx.AttackIndexHash, Ctx.ItemManager.equippedWeapon.animation);
        Ctx.Animator.SetTrigger(Ctx.UseHash);
    }

    private void DamageEnemy()
    {
        var colliders =
            Physics.OverlapSphere(Ctx.ItemManager.equippedWeapon.centerPoint.transform.position, Ctx.CenterPointRadius, Ctx.TargetLayerMask);

        if (colliders.Length > 0)
        {
            // L'ennemi perd de la vie **À COMPLÉTER**
        }
    }
}
