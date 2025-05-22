using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    CharacterMovement characterMovement;
    CharacterAction characterAction;
    void Awake()
    {
        characterMovement = GetComponent<CharacterMovement>();
        characterAction = GetComponent<CharacterAction>();
    }

    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 2, Color.red); // 현재 전방
        Debug.DrawRay(transform.position, characterMovement.currentMoveDirection * 2, Color.blue); // 이동 방향
    }

    void FixedUpdate()
    {
        characterMovement.Move();
    }
}
