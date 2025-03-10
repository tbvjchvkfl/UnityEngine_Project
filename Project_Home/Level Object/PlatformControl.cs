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

    [Header("Value")]
    public float RotateSpeed;

    [HideInInspector] public bool bIsInteracting;


    // ====================================
    //          - Private Data-
    // ====================================
    Transform PlatformWorldTransform;
    Transform TargetCharacterTransform;
    PlayerInput TargetCharacterInput;
    bool bIsRotationStart;

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
    }

    void Update()
    {
        SetPlatformRotation();
        if (bIsInteracting)
        {
            //transform.Translate(TargetCharacterTransform.position);
            transform.position = TargetCharacterTransform.position;//new Vector3(TargetCharacterInput.GetCharacterCapsule().bounds.size.x / 2, 0.0f, 0.0f);
        }
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
}
