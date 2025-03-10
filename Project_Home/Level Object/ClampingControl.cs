using UnityEngine;

public class ClampingControl : MonoBehaviour
{
    // ====================================
    //          - Public Data-
    // ====================================
    [Header("Object")]
    public GameObject TargetCharacter;


    [Header("Value")]
    public int ObjectCount;

    // ====================================
    //          - Private Data-
    // ====================================
    BoxCollider2D BoxCollision;
    Transform TargetTransform;
    PlayerInfo TargetInfo;

    void Awake()
    {
        BoxCollision = GetComponent<BoxCollider2D>();
        if (TargetCharacter)
        {
            TargetTransform = TargetCharacter.GetComponent<Transform>();
            TargetInfo = TargetCharacter.GetComponent<PlayerInfo>();
        }
    }

    void Update()
    {
        ActiveCollision();
    }

    void ActiveCollision()
    {
        if (TargetTransform.position.x < transform.position.x)
        {
            BoxCollision.isTrigger = true;
        }
        else
        {
            BoxCollision.isTrigger = false;
        }
    }

    void CheckEnablePassingTrigger()
    {
        if (TargetTransform.position.x < transform.position.x)
        {
            if (TargetInfo.CurrentStageCount < ObjectCount)
            {
                BoxCollision.isTrigger = false;
                return;
            }
            BoxCollision.isTrigger = true;
        }
        else
        {
            BoxCollision.isTrigger = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (collision.gameObject.GetComponent<PlayerInfo>().CurrentStageCount < ObjectCount)
            {
                Debug.Log("Collision");
            }
        }
    }
}
