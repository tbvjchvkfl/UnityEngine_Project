using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformControl : MonoBehaviour
{
    // ====================================
    //          - Public Data-
    // ====================================

    [Header("Interacting Object")]
    public GameObject TargetCharacter;

    [Header("Object")]
    public GameObject ControlledPlatform;

    [Header("Data Construct")]
    public List<GameObject> TargetObjs;

    [Header("Value")]
    public float ObjectLoc;
    public float ReturnSpeed;
    public float MaxPullDistance;
    public bool bIsInteracting { get; set; }
    public bool bIsPullMax {  get; private set; }

    // ====================================
    //          - Private Data-
    // ====================================
    Transform PlatformWorldTransform;
    Transform TargetCharacterTransform;
    Vector3 InitPos;
    PlayerInput TargetCharacterInput;

    void Awake()
    {
        if (ControlledPlatform)
        {
            PlatformWorldTransform = ControlledPlatform.GetComponent<Transform>();
        }
        if (TargetCharacter)
        {
            TargetCharacterTransform = TargetCharacter.gameObject.GetComponent<Transform>();
            TargetCharacterInput = TargetCharacter.GetComponent<PlayerInput>();
        }
        InitPos = transform.position;
    }

    void Update()
    {
        MovePullTrigger();
    }

    void MovePullTrigger()
    {
        if (bIsInteracting)
        {
            transform.position = new Vector3(TargetCharacterTransform.position.x + ObjectLoc, transform.position.y, transform.position.z);
            if (transform.position.x <= InitPos.x - MaxPullDistance)
            {
                bIsPullMax = true;
                transform.position = new Vector3(InitPos.x - MaxPullDistance, transform.position.y, transform.position.z);
                for (int i = 0; i < TargetObjs.Count; i++)
                {
                    if (TargetObjs[i].GetComponent<BoxCollider2D>())
                    {
                        TargetObjs[i].GetComponent<SpriteRenderer>().color += new Color(Time.deltaTime, Time.deltaTime, Time.deltaTime, 1.0f);
                        if (TargetObjs[i].GetComponent<SpriteRenderer>().color.r >= 1.0f)
                        {
                            TargetObjs[i].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                            TargetObjs[i].GetComponent<BoxCollider2D>().isTrigger = false;
                            TargetObjs[i].GetComponent<BoxCollider2D>().enabled = true;
                        }
                    }
                }
            }
            else
            {
                bIsPullMax = false;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, InitPos, ReturnSpeed * Time.deltaTime);
            if (transform.position.x >= InitPos.x - 0.03f)
            {
                transform.position = InitPos;
                for (int i = 0; i < TargetObjs.Count; i++)
                {
                    if (TargetObjs[i].GetComponent<BoxCollider2D>())
                    {
                        TargetObjs[i].GetComponent<SpriteRenderer>().color -= new Color(Time.deltaTime, Time.deltaTime, Time.deltaTime, 0.0f);
                        if (TargetObjs[i].GetComponent<SpriteRenderer>().color.r <= 0.6f)
                        {
                            TargetObjs[i].GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.6f, 0.6f, 1.0f);
                            TargetObjs[i].GetComponent<BoxCollider2D>().isTrigger = true;
                            TargetObjs[i].GetComponent<BoxCollider2D>().enabled = false;
                        }
                    }
                }
            }
        }
    }
}
