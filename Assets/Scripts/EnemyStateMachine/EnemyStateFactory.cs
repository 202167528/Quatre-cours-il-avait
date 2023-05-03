using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateFactory
{
    private EnemyStateMachine context;
    private Dictionary<EnemyStates, EnemyBaseState> states = new Dictionary<EnemyStates, EnemyBaseState>();
    
    public EnemyStateFactory(EnemyStateMachine currentContext)
    {
        context = currentContext;
        states[EnemyStates.chase] = new EnemyWalkState(context, this);
        states[EnemyStates.run] = new EnemyRunState(context, this);
        states[EnemyStates.attack] = new EnemyAttackState(context, this);
        states[EnemyStates.walk] = new EnemyWalkState(context, this);
        states[EnemyStates.wait] = new EnemyWaitState(context, this);
        states[EnemyStates.chase] = new EnemyChaseState(context, this);
        states[EnemyStates.patrol] = new EnemyPatrolState(context, this);
    }

    public EnemyBaseState Walk() { return states[EnemyStates.walk]; }
    
    public EnemyBaseState Run() { return states[EnemyStates.run];}

    public EnemyBaseState Attack() { return states[EnemyStates.attack];}
    
    public EnemyBaseState Wait() { return states[EnemyStates.wait];}

    public EnemyBaseState Chase() { return states[EnemyStates.chase]; }

    public EnemyBaseState Patrol() { return states[EnemyStates.patrol]; }
}

public enum EnemyStates
{
    walk, run, attack, wait, patrol, chase
}
