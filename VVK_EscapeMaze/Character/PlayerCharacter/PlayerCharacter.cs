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
        
    }

    void FixedUpdate()
    {
        characterMovement.Move();
    }
}
