using System.Collections;
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
        Debug.Log("Idle");
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
        stateMachine.animationController.SetTrigger("Roar");
        Debug.Log("Move");
    }

    public void LoopState(EnemyStateMachine stateMachine)
    {
        if (!stateMachine.animationController.GetCurrentAnimatorStateInfo(0).IsName("Base Lyaer.Attack State"))
        {
            SetMoveSpeed(stateMachine);
            if (stateMachine.bIsRecognize)
            {
                ChaseTargetLocation(stateMachine);
            }
            else
            {
                MovetoRandomLocation(stateMachine);
            }
        }
        else
        {
            stateMachine.navAgent.speed = 0.0f;
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

    void SetMoveSpeed(EnemyStateMachine owner)
    {
        owner.navAgent.speed = 1.5f;
        if (owner.bIsRecognize)
        {
            owner.animationController.SetTrigger("Roar");
            owner.navAgent.speed = 4.0f;
        }
    }
}

public class AttackState : IEnemyState
{
    Coroutine playAttackStateCoroutine;

    public void EnterState(EnemyStateMachine stateMachine)
    {
        stateMachine.bIsAttack = false;
        stateMachine.navAgent.speed = 0.0f;
    }

    public void LoopState(EnemyStateMachine stateMachine)
    {
        if (!stateMachine.navAgent.pathPending && Vector3.Distance(stateMachine.transform.position, stateMachine.TargetLocation) <= 3.0f)
        {
            Debug.Log("Attack");
            if (playAttackStateCoroutine == null)
            {
                Vector3 targetDir = stateMachine.TargetLocation - stateMachine.transform.position;
                targetDir.y = 0.0f;
                targetDir.Normalize();

                stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation, Quaternion.LookRotation(targetDir), stateMachine.navAgent.angularSpeed * Time.deltaTime);

                stateMachine.bIsAttack = true;
                playAttackStateCoroutine = stateMachine.StartCoroutine(AttackCooldown(stateMachine));
            }
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

        if(playAttackStateCoroutine != null)
        {
            stateMachine.StopCoroutine(playAttackStateCoroutine);
            playAttackStateCoroutine = null;
        }
    }

    IEnumerator AttackCooldown(EnemyStateMachine Owner)
    {
        yield return null;
        yield return new WaitForSeconds(Owner.animationController.GetCurrentAnimatorStateInfo(0).length + 1.0f);
        Owner.ModifyState(new IdleState());
        playAttackStateCoroutine = null;
    }
}

public class ReactState : IEnemyState
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
    public bool bIsHit { get; set; } = false;

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
        animationController.SetBool("Hit", bIsHit);

        //animationController.SetFloat("Idle State Index", Random.Range(0, 2));
        animationController.SetFloat("Moving Index", bIsRecognize ? 1 : 0);
    }

    public void ModifyState(IEnemyState newState)
    {
        if (currentState.GetType() != newState.GetType())
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

    public void OnTakeDamage(float damage)
    {
        if (damage > 0.0f)
        {
            animationController.SetLayerWeight(1, 0.35f);
            bIsHit = true;
            StartCoroutine(OnHit());

            // 체력 관련 로직 추가
        }
    }

    IEnumerator OnHit()
    {
        yield return new WaitForSeconds(0.25f);
        bIsHit = false;
        animationController.SetLayerWeight(1, 0.0f);
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
