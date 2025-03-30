using UnityEngine;

public class MovingElevator : MonoBehaviour
{
    public float TargetPositionValue;
    public GameObject ElevatorDoor;

    [HideInInspector] public bool bIsInteraction;

    Transform ObjTransform;
    Transform TargetTransform;
    Transform DoorTransform;

    Vector3 TargetLocation;
    Vector3 OriginalLocation;

    Vector3 DoorClosePos;
    Vector3 DoorOpenPos;

    float PlatformMovingSpeed;
    bool bIsArrived;

    void Awake()
    {
        ObjTransform = GetComponent<Transform>();
        OriginalLocation = ObjTransform.position;
        TargetLocation = new Vector3(OriginalLocation.x, OriginalLocation.y + TargetPositionValue, OriginalLocation.z);
        DoorTransform = ElevatorDoor.GetComponent<Transform>();
        DoorOpenPos = DoorTransform.localPosition;
        DoorClosePos = new Vector3(DoorOpenPos.x, 0.0f, DoorOpenPos.z);
        DoorOpen();
    }

    void Update()
    {
        if (bIsInteraction)
        {
            Debug.Log(ObjTransform.position.y);
            Debug.Log(TargetLocation.y);
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
                    PlatformMovingSpeed += 0.002f;
                    if (PlatformMovingSpeed >= 0.3f)
                    {
                        PlatformMovingSpeed = 0.3f;
                    }
                    ObjTransform.position = Vector3.Lerp(ObjTransform.position, TargetLocation, Time.deltaTime * PlatformMovingSpeed);
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
                    PlatformMovingSpeed += 0.002f;
                    if (PlatformMovingSpeed >= 0.3f)
                    {
                        PlatformMovingSpeed = 0.3f;
                    }
                    ObjTransform.position = Vector3.Lerp(ObjTransform.position, OriginalLocation, Time.deltaTime * PlatformMovingSpeed);
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
