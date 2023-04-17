using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractState : PlayerBaseState
{
    public PlayerInteractState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory) {}

    public override void EnterState()
    {
        HandleUse();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Ctx.Animator.SetBool(Ctx.IsInteractingHash,false);
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsInteracting)
        {
            switch (Ctx.IsMovementPressed)
            {
                case false:
                    SwitchState(Factory.Idle());
                    break;
                case true when !Ctx.IsRunPressed:
                    SwitchState(Factory.Walk());
                    break;
                case true when Ctx.IsRunPressed:
                    SwitchState(Factory.Run());
                    break;
            }
        }
    }

    private void HandleUse()
    {
        var items = Physics.OverlapSphere(Ctx.CharacterController.transform.position, 1.0f, Ctx.WeaponLayerMask);
        if (items.Length != 0)
        {
            Ctx.AppliedMovementX = 0; 
            Ctx.AppliedMovementZ = 0;
            
            if (items[0].gameObject.GetComponent<WeaponScript>().WeaponData)
            {
                Ctx.WeaponData = items[0].gameObject.GetComponent<WeaponScript>().WeaponData;
                Ctx.ItemManager.EquipWeapon(Ctx.WeaponData);
            } else if (items[0].gameObject.GetComponent<Potion>().PotionData)
            {
                Ctx.PotionData = items[0].gameObject.GetComponent<Potion>().PotionData;
                Ctx.ItemManager.EquipPotion(Ctx.PotionData);
            }
            
            Ctx.Animator.SetBool(Ctx.IsInteractingHash,true);
            Ctx.IsInteracting = true;
                
            Ctx.ItemManager.DestroyGameObject(items[0].gameObject);
        }
        else
        {
            Ctx.IsInteracting = false;
        }
    }

    public override void InitializeSubState(){}
}
