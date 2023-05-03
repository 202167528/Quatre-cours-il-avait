using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState
{
    private bool isRootState;
    private EnemyStateMachine ctx;
    private EnemyStateFactory factory;
    private EnemyBaseState currentSuperState;
    private EnemyBaseState currentSubState;

    protected bool IsRootState { set => isRootState = value; }
    protected EnemyStateMachine Ctx { get => ctx; }
    protected EnemyStateFactory Factory { get => factory; }
    
    public EnemyBaseState(EnemyStateMachine currentContext, EnemyStateFactory playerStateFactory)
    {
        ctx = currentContext;
        factory = playerStateFactory;
    }
    
    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract  void ExitState();

    public abstract void CheckSwitchStates();

    public abstract void InitializeSubState();

    public void UpdateStates()
    {
        UpdateState();
        if (currentSubState != null)
        {
            currentSubState.UpdateStates();
        }
    }

    protected void SwitchState(EnemyBaseState newState)
    {
        ExitState();
        
        // nouveau état entre dans État
        newState.EnterState();

        // change l'état présentement du contexte
        if (isRootState)
        {
            ctx.CurrentState = newState;
        } else if (currentSuperState != null)
        {
            currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(EnemyBaseState newSuperState)
    {
        currentSuperState = newSuperState;
    }

    protected void SetSubState(EnemyBaseState newSubState)
    {
        currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
