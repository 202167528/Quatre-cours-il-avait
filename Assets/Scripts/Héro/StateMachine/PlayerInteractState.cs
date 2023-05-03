using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractState : PlayerBaseState
{
    public PlayerInteractState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) : base(
        currentContext, playerStateFactory) {}

    public override void EnterState()
    {
        HandleInteract();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
    }

    public override void CheckSwitchStates()
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

    private void HandleInteract()
    {
        var items = Physics.OverlapSphere(Ctx.CharacterController.transform.position, Ctx.ItemDetectionRadius, Ctx.ItemLayerMask);
        if (items.Length != 0)
        {
            if (Ctx.ItemManager.equippedWeapon != null)
            {
                if (items[0].gameObject.GetComponent<Potion>())
                {
                    Ctx.PotionData = items[0].gameObject.GetComponent<Potion>().PotionData;
                    Ctx.ItemManager.EquipPotion(Ctx.PotionData);
                }
                return;
            }
            if (items[0].gameObject.GetComponent<WeaponScript>())
            {
                Ctx.WeaponData = items[0].gameObject.GetComponent<WeaponScript>().WeaponData;
                Ctx.ItemManager.EquipWeapon(Ctx.WeaponData);
            }
            
            Ctx.ItemManager.DestroyGameObject(items[0].gameObject);
        }
    }

    public override void InitializeSubState(){}
}
