using System;using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateFactory
{
    private PlayerStateMachine context;
    private Dictionary<PlayerStates, PlayerBaseState> states = new Dictionary<PlayerStates, PlayerBaseState>();

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        context = currentContext;
        states[PlayerStates.idle] = new PlayerIdleState(context, this);
        states[PlayerStates.walk] = new PlayerWalkState(context, this);
        states[PlayerStates.run] = new PlayerRunState(context, this);
        states[PlayerStates.interact] = new PlayerInteractState(context, this);
        states[PlayerStates.jump] = new PlayerJumpState(context, this);
        states[PlayerStates.grounded] = new PlayerGroundedState(context, this);
        states[PlayerStates.fall] = new PlayerFallState(context, this);
        states[PlayerStates.aim] = new PlayerAimState(context, this);
        states[PlayerStates.use] = new PlayerUseState(context, this);
    }

    public PlayerBaseState Idle() { return states[PlayerStates.idle]; }

    public PlayerBaseState Walk() { return states[PlayerStates.walk]; }

    public PlayerBaseState Run() { return states[PlayerStates.run]; }

    public PlayerBaseState Jump() { return states[PlayerStates.jump]; }
    
    public PlayerBaseState Interact() { return states[PlayerStates.interact]; }

    public PlayerBaseState Grounded() { return states[PlayerStates.grounded]; }

    public PlayerBaseState Fall() { return states[PlayerStates.fall]; }
    
    public PlayerBaseState Aim() { return states[PlayerStates.aim]; }
    
    public PlayerBaseState Use() { return states[PlayerStates.use]; }
}

public enum PlayerStates
{
    idle, walk, run, interact, grounded, jump, fall, aim, use
}
