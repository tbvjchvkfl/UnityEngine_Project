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
    float DelayTime = 0.0f;
    public void EnterState(EnemyStateMachine stateMachine)
    {
        stateMachine.FindRandomLocation();
    }

    public void LoopState(EnemyStateMachine stateMachine)
    {
        DelayTime += Time.deltaTime;
        if (DelayTime > 1.5f)
        {
            if (stateMachine.bIsRecognize)
            {
                stateMachine.ModifyState(new AttackState());
            }
            else
            {
                stateMachine.ModifyState(new MoveState());
            }
        }
    }

    public void ExitState(EnemyStateMachine stateMachine)
    {
        DelayTime = 0.0f;
    }
}

public class MoveState : IEnemyState
{
    public void EnterState(EnemyStateMachine stateMachine)
    {

    }

    public void LoopState(EnemyStateMachine stateMachine)
    {
        if (stateMachine.bIsRecognize)
        {

        }
        else
        {
            stateMachine.MovetoTargetLocation(stateMachine.desiredMoveLocation);
            if (Vector3.Distance(stateMachine.transform.position, stateMachine.desiredMoveLocation) <= 2.0f)
            {
                stateMachine.ModifyState(new IdleState());
            }
        }
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
    GameObject TargetEnemy;
    SphereCollider perceptionCollider;
    EnemyBase characterBase;

    IEnemyState currentState;
    NavMeshAgent navAgent;

    public Vector3 desiredMoveLocation { get; private set; } = Vector3.zero;
    public Vector3 TargetLocation { get; private set; } = Vector3.zero;

    public bool bIsRecognize { get; private set; } = false;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        perceptionCollider = GetComponent<SphereCollider>();
        characterBase = GetComponent<EnemyBase>();


        currentState = new IdleState();
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.LoopState(this);
        Debug.DrawLine(transform.position, desiredMoveLocation, Color.red);
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

    public void FindRandomLocation()
    {
        Vector3 RandomLocation = Random.insideUnitSphere * perceptionCollider.radius;
        RandomLocation.y = 0.0f;

        Vector3 CorePosition = transform.position + RandomLocation;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(CorePosition, out hit, perceptionCollider.radius, NavMesh.AllAreas))
        {
            desiredMoveLocation = hit.position;
        }
        //desiredMoveLocation = RandomLocation;
    }

    public void MovetoTargetLocation(Vector3 targetLocation)
    {
        navAgent.SetDestination(targetLocation);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bIsRecognize = true;
            perceptionCollider.radius = 10.0f;
            TargetEnemy = other.gameObject;
            TargetLocation = other.gameObject.transform.position;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TargetEnemy = null;
            bIsRecognize = false;
            perceptionCollider.radius = 3.5f;
        }
    }
}
