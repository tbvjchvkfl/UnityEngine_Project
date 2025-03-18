using UnityEngine;

public class MovingElevator : MonoBehaviour
{
    public float TargetPositionValue;
    public float PlatformMovingSpeed;

    [HideInInspector] public bool bIsInteraction;

    Transform ObjTransform;
    Transform TargetTransform;

    Vector3 TargetLocation;
    Vector3 OriginalLocation;

    void Awake()
    {
        ObjTransform = GetComponent<Transform>();
        OriginalLocation = ObjTransform.position;
        TargetLocation = new Vector3(OriginalLocation.x, OriginalLocation.y + TargetPositionValue, OriginalLocation.z);
    }

    void Update()
    {
        if (bIsInteraction)
        {
            ObjTransform.position = Vector3.Lerp(ObjTransform.position, TargetLocation, Time.deltaTime * PlatformMovingSpeed);
            if (ObjTransform.position.y <= TargetLocation.y)
            {
                ObjTransform.position = TargetLocation;
            }
        }
        else
        {

        }
    }
}
