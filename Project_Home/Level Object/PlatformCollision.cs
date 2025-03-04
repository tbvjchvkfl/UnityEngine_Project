using UnityEngine;

public class PlatformCollision : MonoBehaviour
{
    // ====================================
    //          - Public Data-
    // ====================================
    [Header("Component")]
    public GameObject CollisionFirstObj;
    public GameObject CollisionSecondObj;
    public GameObject CollisionThirdObj;

    public GameObject TargetCharacter;



    // ====================================
    //          - Private Data-
    // ====================================

    // Component
    Transform PlayerCharacterPos;
    Transform FirstCollisionPos;
    Transform SecondCollisionPos;
    Transform ThirdCollisionPos;

    BoxCollider2D FirstCollisionBox;
    BoxCollider2D SecondCollisionBox;
    BoxCollider2D ThirdCollisionBox;


    void Awake()
    {
        if (TargetCharacter)
        {
            PlayerCharacterPos = TargetCharacter.GetComponent<Transform>();
        }
        if (CollisionFirstObj)
        {
            FirstCollisionPos = CollisionFirstObj.GetComponent<Transform>();
            FirstCollisionBox = CollisionFirstObj.GetComponent<BoxCollider2D>();
        }
        if (CollisionSecondObj)
        {
            SecondCollisionPos = CollisionSecondObj.GetComponent<Transform>();
            SecondCollisionBox = CollisionSecondObj.GetComponent<BoxCollider2D>();
        }
        if (CollisionThirdObj)
        {
            ThirdCollisionPos = CollisionThirdObj.GetComponent<Transform>();
            ThirdCollisionBox = CollisionThirdObj.GetComponent<BoxCollider2D>();
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        CheckTargetPosition();
    }

    void CheckTargetPosition()
    {
        if (PlayerCharacterPos)
        {
            if (FirstCollisionPos)
            {
                if (PlayerCharacterPos.position.y < FirstCollisionPos.position.y)
                {
                    FirstCollisionBox.isTrigger = true;
                }
                else
                {
                    FirstCollisionBox.isTrigger = false;
                }
            }
            if (SecondCollisionPos)
            {
                if (PlayerCharacterPos.position.y < SecondCollisionPos.position.y)
                {
                    SecondCollisionBox.isTrigger = true;
                }
                else
                {
                    SecondCollisionBox.isTrigger = false;
                }
            }
            if (ThirdCollisionPos)
            {
                if (PlayerCharacterPos.position.y < ThirdCollisionPos.position.y)
                {
                    ThirdCollisionBox.isTrigger = true;
                }
                else
                {
                    ThirdCollisionBox.isTrigger = false;
                }
            }
        }
    }
}
