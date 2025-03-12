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
    float PreviousPos;

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
        PreviousPos = Mathf.Clamp(Mathf.Abs(InitPos.x), 0.0f, 1.0f);
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
        
        if (bIsInteracting)
        {
            transform.position = new Vector3(TargetCharacterTransform.position.x + ObjectLoc, transform.position.y, transform.position.z);
            CaculatePercent();
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
            CaculatePercent();
        }
    }

    void CaculatePercent()
    {
        float ClampingCurrentPos = Mathf.Clamp(Mathf.Abs(transform.position.x), 0.0f, 1.0f);
        if (ClampingCurrentPos < PreviousPos)
        {
            for (int i = 0; i < TargetObjs.Count; i++)
            {
                TargetObjs[i].GetComponent<SpriteRenderer>().color += new Color(0.02f, 0.02f, 0.02f, 1.0f);
                if (TargetObjs[i].GetComponent<SpriteRenderer>().color.r >= 1.0f)
                {
                    TargetObjs[i].GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                }
            }
        }
        else if (ClampingCurrentPos > PreviousPos)
        {
            for (int i = 0; i < TargetObjs.Count; i++)
            {
                TargetObjs[i].GetComponent<SpriteRenderer>().color -= new Color(0.02f, 0.02f, 0.02f, 0.0f);
                if (TargetObjs[i].GetComponent<SpriteRenderer>().color.r <= 0.6f)
                {
                    TargetObjs[i].GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.6f, 0.6f, 1.0f);
                }
            }
        }
        PreviousPos = ClampingCurrentPos;
    }
}
