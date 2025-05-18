using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    [Header("Locomotion")]
    

    [Header("Component")]



    // Component
    PCInputManager inputManager;
    CharacterController characterController;

    // Locomotion Value
    

    void Awake()
    {
        inputManager = GetComponent<PCInputManager>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {

    }
}
