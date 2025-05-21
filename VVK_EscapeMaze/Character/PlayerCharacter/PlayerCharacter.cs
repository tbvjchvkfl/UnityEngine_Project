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
        Vector3 origin = transform.position + Vector3.up * 1.0f; // 시야 중심 위치에서 시작
        Vector3 forward = transform.forward * 50.0f;

        Debug.DrawLine(origin, origin + forward, Color.green);
    }

    void FixedUpdate()
    {
        characterMovement.Move();
    }
}
