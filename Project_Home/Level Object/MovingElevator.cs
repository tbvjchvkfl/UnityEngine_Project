using UnityEngine;

public class MovingElevator : MonoBehaviour
{

    public GameObject ElevatorDoor;

    public float TargetPositionValue;

    [HideInInspector] public bool bIsInteraction;

    Transform ObjTransform;
    Transform DoorTransform;

    Vector3 TargetLocation;
    Vector3 OriginalLocation;

    Vector3 DoorClosePos;
    Vector3 DoorOpenPos;

    float PlatformMovingSpeed;

    void Awake()
    {
        ObjTransform = GetComponent<Transform>();
        OriginalLocation = ObjTransform.position;
        TargetLocation = new Vector3(OriginalLocation.x, OriginalLocation.y + TargetPositionValue, OriginalLocation.z);
        DoorTransform = ElevatorDoor.GetComponent<Transform>();
        DoorOpenPos = DoorTransform.localPosition;
        DoorClosePos = new Vector3(DoorOpenPos.x, 0.0f, DoorOpenPos.z);
        PlatformMovingSpeed = 0.0f;
        DoorOpen();
    }

    void Update()
    {
        if (bIsInteraction)
        {
            if (ObjTransform.position.y <= -52.7f)
            {
                ObjTransform.position = TargetLocation;
                PlatformMovingSpeed = 0.0f;
                DoorOpen();
            }
            else
            {
                if (DoorClose())
                {
                    PlatformMovingSpeed += 0.02f;
                    if (PlatformMovingSpeed >= 7.0f)
                    {
                        PlatformMovingSpeed = 7.0f;
                    }
                    GameManager.Instance.PCInfo.gameObject.transform.Translate(Vector3.down * PlatformMovingSpeed * Time.deltaTime);
                    ObjTransform.Translate(Vector3.down * PlatformMovingSpeed * Time.deltaTime);
                }
            }
        }
        else
        {
            if (ObjTransform.position.y >= OriginalLocation.y + -0.05f)
            {
                ObjTransform.position = OriginalLocation;
                PlatformMovingSpeed = 0.0f;
                DoorOpen();
            }
            else
            {
                if (DoorClose())
                {
                    PlatformMovingSpeed += 0.02f;
                    if (PlatformMovingSpeed >= 7.0f)
                    {
                        PlatformMovingSpeed = 7.0f;
                    }
                    GameManager.Instance.PCInfo.gameObject.transform.Translate(Vector3.down * PlatformMovingSpeed * Time.deltaTime);
                    ObjTransform.Translate(Vector3.up * PlatformMovingSpeed * Time.deltaTime);
                }
            }
        }
    }

    void DoorOpen()
    {
        DoorTransform.Translate(Vector2.up * 0.03f, Space.World);
        if (DoorTransform.localPosition.y >= DoorOpenPos.y)
        {
            DoorTransform.localPosition = DoorOpenPos;
        }
    }

    bool DoorClose()
    {
        DoorTransform.Translate(Vector2.down * 0.03f, Space.World);
        if (DoorTransform.localPosition.y <= DoorClosePos.y)
        {
            DoorTransform.localPosition = DoorClosePos;
            return true;
        }
        return false;
    }
}
