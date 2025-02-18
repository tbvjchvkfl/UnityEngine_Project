using UnityEngine;
using System.Collections;

public enum CharacterState
{
    Idle,
    Move,
    Attack,
    Cast,
    Spell,
    Hit,
    Death
}

public class Boss : MonoBehaviour
{
    Rigidbody2D rigidbody2;
    BoxCollider2D boxCollider2;
    Animator animator;
    private CharacterState characterstate;

    private void Awake()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();
        boxCollider2 = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        characterstate = CharacterState.Idle;
    }

    void Start()
    {
        ModifyingState(characterstate);
    }

    void Update()
    {
        // 상태별 ChangeState호출
        DoAttack();




        //UpdateState();
    }

    void FixedUpdate()
    {
        
    }

    void ModifyingState(CharacterState NewState)
    {
        if (characterstate == NewState)
        {
            return;
        }
        StopCoroutine(characterstate.ToString());
        characterstate = NewState;
        StartCoroutine(characterstate.ToString());
    }

    void DoAttack()
    {
        Collider2D HitEnemy = Physics2D.OverlapBox(boxCollider2.bounds.center, boxCollider2.bounds.size, 0.0f);
        if (HitEnemy)
        {
            Debug.Log("Hit!");
        }
        if (HitEnemy.gameObject.tag == "Player")
        {
            ModifyingState(CharacterState.Attack);
        }
        else
        {
            ModifyingState(CharacterState.Idle);
        }
    }

    IEnumerator Idle()
    {
        Debug.Log("Idle");


        while (true)
        {
            Debug.Log("Idle Looping");
            yield return null;
        }
    }

    IEnumerator Move()
    {


        while (true)
        {
            yield return null;
        }
    }

    IEnumerator Attack()
    {


        while (true)
        {
            Debug.Log("Attack");
            animator.SetTrigger("IsAttacking");
            yield return new WaitForSeconds(3.0f);
        }
    }

    void UpdateState()
    {
        switch (characterstate)
        {
            case CharacterState.Idle:
                {
                    
                }
                break;
            case CharacterState.Move:
                {

                }
                break;
            case CharacterState.Attack:
                {

                }
                break;
            case CharacterState.Cast:
                {

                }
                break;
            case CharacterState.Spell:
                {

                }
                break;
            case CharacterState.Hit:
                {

                }
                break;

        }
    }
}
