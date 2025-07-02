using UnityEngine;
using UnityEngine.AI;

public interface IEnemyState
{
    public void EnterState(EnemyStateMachine stateMachine);
    public void LoopState(EnemyStateMachine stateMachine);
    public void ExitState(EnemyStateMachine stateMachine);
}

public class IdleState : IEnemyState
{
    public void EnterState(EnemyStateMachine stateMachine)
    {

    }

    public void LoopState(EnemyStateMachine stateMachine)
    {

    }

    public void ExitState(EnemyStateMachine stateMachine)
    {

    }
}

public class MoveState : IEnemyState
{
    public void EnterState(EnemyStateMachine stateMachine)
    {

    }

    public void LoopState(EnemyStateMachine stateMachine)
    {

    }

    public void ExitState(EnemyStateMachine stateMachine)
    {

    }
}

public class AttackState : IEnemyState
{
    public void EnterState(EnemyStateMachine stateMachine)
    {

    }

    public void LoopState(EnemyStateMachine stateMachine)
    {

    }

    public void ExitState(EnemyStateMachine stateMachine)
    {

    }
}

public class DeathState : IEnemyState
{
    public void EnterState(EnemyStateMachine stateMachine)
    {

    }

    public void LoopState(EnemyStateMachine stateMachine)
    {

    }

    public void ExitState(EnemyStateMachine stateMachine)
    {

    }
}

public class EnemyStateMachine : MonoBehaviour
{
    IEnemyState currentState;
    NavMeshAgent navAgent;
    public GameObject TargetEnemy;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        currentState = new IdleState();
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.LoopState(this);
        navAgent.SetDestination(TargetEnemy.transform.position);
    }

    public void ModifyState(IEnemyState newState)
    {
        if (currentState.ToString() != newState.ToString())
        {
            currentState.ExitState(this);
            currentState = newState;
            currentState.EnterState(this);
        }
    }
}
