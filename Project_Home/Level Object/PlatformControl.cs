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
    public float RotateSpeed;
    public float ObjectLoc;
    public float ReturnSpeed;


    [HideInInspector] public bool bIsInteracting;


    // ====================================
    //          - Private Data-
    // ====================================
    Transform PlatformWorldTransform;
    Transform TargetCharacterTransform;
    Vector3 InitPos;
    PlayerInput TargetCharacterInput;
    bool bIsRotationStart;
    bool PullMax;
    

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
        SetPlatformRotation();
        MovePullTrigger();
    }

    void SetPlatformRotation()
    {
        if (bIsRotationStart)
        {
            if (PlatformWorldTransform.eulerAngles.z <= 40.0f)
            {
                PlatformWorldTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 40.0f);
                return;
            }
            PlatformWorldTransform.Rotate(new Vector3(0.0f, 0.0f, -1.0f * RotateSpeed * Time.deltaTime));
        }
        else
        {
            if (PlatformWorldTransform.eulerAngles.z >= 320.0f)
            {
                PlatformWorldTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 320.0f);
                return;
            }
            PlatformWorldTransform.Rotate(new Vector3(0.0f, 0.0f, RotateSpeed * Time.deltaTime));
        }
    }

    public void StartPlatformRotation()
    {
        if (!bIsRotationStart)
        {
            bIsRotationStart = true;
        }
        else
        {
            bIsRotationStart = false;
        }
    }

    public bool GetPullMax()
    {
        return PullMax;
    }

    void MovePullTrigger()
    {
        CaculatePercent();
        if (bIsInteracting)
        {
            transform.position = new Vector3(TargetCharacterTransform.position.x + ObjectLoc, transform.position.y, transform.position.z);

            if (transform.position.x <= 0.2f)
            {
                transform.position = new Vector3(0.2f, transform.position.y, transform.position.z);
                PullMax = true;
            }
            else
            {
                PullMax = false;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, InitPos, ReturnSpeed * Time.deltaTime);
        }
    }

    void CaculatePercent()
    {
        float MaxValue = Mathf.Abs(transform.position.x * 100.0f);
        Debug.Log(MaxValue);
    }
}
