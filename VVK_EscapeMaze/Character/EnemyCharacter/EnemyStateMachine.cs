using Unity.VisualScripting;
using UnityEditorInternal;
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
        DelayTime = 0.0f;
        stateMachine.FindDestination();
    }

    public void LoopState(EnemyStateMachine stateMachine)
    {
        DelayTime += Time.deltaTime;
        if (DelayTime > 1.0f)
        {
            if (stateMachine.bIsRecognize)
            {
                if (!stateMachine.navAgent.pathPending && stateMachine.navAgent.remainingDistance <= stateMachine.navAgent.stoppingDistance)
                {
                    stateMachine.ModifyState(new AttackState());
                }
                else
                {
                    stateMachine.ModifyState(new MoveState());
                }
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
        if (stateMachine.bIsRecognize)
        {
            stateMachine.animationController.SetTrigger("Roar");
        }
    }

    public void LoopState(EnemyStateMachine stateMachine)
    {
        if (stateMachine.bIsRecognize)
        {
            ChaseTargetLocation(stateMachine);
        }
        else
        {
            MovetoRandomLocation(stateMachine);
        }
    }

    public void ExitState(EnemyStateMachine stateMachine)
    {

    }

    void ChaseTargetLocation(EnemyStateMachine SM)
    {
        SM.SetCharacterRotation();
        SM.navAgent.SetDestination(SM.TargetLocation);
        if (!SM.navAgent.pathPending && SM.navAgent.remainingDistance <= SM.navAgent.stoppingDistance)
        {
            SM.ModifyState(new AttackState());
        }
    }

    void MovetoRandomLocation(EnemyStateMachine SM)
    {
        SM.SetCharacterRotation();
        SM.navAgent.SetDestination(SM.desiredMoveLocation);
        if (!SM.navAgent.pathPending && SM.navAgent.remainingDistance <= SM.navAgent.stoppingDistance)
        {
            SM.ModifyState(new IdleState());
        }
    }
}

public class AttackState : IEnemyState
{
    public void EnterState(EnemyStateMachine stateMachine)
    {
        stateMachine.bIsAttack = false;
    }

    public void LoopState(EnemyStateMachine stateMachine)
    {
        if (!stateMachine.navAgent.pathPending && Vector3.Distance(stateMachine.transform.position, stateMachine.TargetLocation) <= 2.0f)
        {
            Debug.Log("Attack");
            stateMachine.bIsAttack = true;
            stateMachine.ModifyState(new IdleState());
        }
        else
        {
            Debug.Log("Not Distance");
            stateMachine.bIsAttack = false;
            stateMachine.ModifyState(new MoveState());
        }
    }

    public void ExitState(EnemyStateMachine stateMachine)
    {
        if (stateMachine.bIsAttack)
        {
            stateMachine.bIsAttack = false;
        }
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

    public NavMeshAgent navAgent { get; private set; }
    public Animator animationController { get; private set; }

    public Vector3 desiredMoveLocation { get; private set; } = Vector3.zero;
    public Vector3 TargetLocation { get; private set; } = Vector3.zero;

    public bool bIsRecognize { get; set; } = false;
    public bool bIsAttack { get; set; } = false;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        perceptionCollider = GetComponent<SphereCollider>();
        animationController = GetComponent<Animator>();
        characterBase = GetComponent<EnemyBase>();

        navAgent.updateRotation = false;
        navAgent.angularSpeed = 100.0f;

        currentState = new IdleState();
        currentState.EnterState(this);
    }

    void Update()
    {
        SetAnimData();
        currentState.LoopState(this);
        Debug.DrawLine(transform.position, desiredMoveLocation, Color.red);
    }

    void SetAnimData()
    {
        animationController.SetBool("Move", navAgent.velocity.magnitude > 0.1f);
        animationController.SetBool("Attack", bIsAttack);

        //animationController.SetFloat("Idle State Index", Random.Range(0, 2));
        animationController.SetFloat("Moving Index", bIsRecognize ? 1 : 0);
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

    public void FindDestination()
    {
        if (bIsRecognize)
        {
            if (TargetEnemy)
            {
                desiredMoveLocation = TargetEnemy.transform.position;
            }
        }
        else
        {
            Vector3 RandomLocation = Random.insideUnitSphere * perceptionCollider.radius;
            RandomLocation.y = 0.0f;

            Vector3 CorePosition = transform.position + RandomLocation;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(CorePosition, out hit, perceptionCollider.radius, NavMesh.AllAreas))
            {
                desiredMoveLocation = hit.position;
            }
        }
    }

    public void SetCharacterRotation()
    {
        Vector3 MovementDirection = navAgent.velocity;
        MovementDirection.y = 0.0f;

        if( MovementDirection.magnitude > 0.1f )
        {
            Quaternion targetRotation = Quaternion.LookRotation(MovementDirection.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, navAgent.angularSpeed * Time.deltaTime);
        }
    }

    public void OnAttackBinding()
    {

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
