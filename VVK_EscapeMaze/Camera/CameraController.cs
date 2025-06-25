using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Property")]
    public GameObject TargetObj;
    public GameObject CameraObj;
    public GameObject AimTarget;
    public GameObject PlayerInventory;
    public float CameraRotationSpeed; // 1보다 높아지면 과하게 빨라짐 왠만하면 0~1값을 유지할 것
    public float ZoomSpeed = 3.0f;
    public float CameraMoveSpeed = 2.0f;
    public Transform InventoryTargetTransform;


    PCInputManager InputManager;
    
    Vector3 CameraRotateDirection;
    Vector3 CameraInitPos;
    Quaternion CameraInitRot;
    Quaternion SpringArmLocalRot;
    float YawAxisValue;
    float PitchAxisValue;
    float ZoomFactor = -3.0f;
    bool bIsMoveSuccess = false;

    LayerMask aimLayerMask;

    void Awake()
    {
        InputManager = TargetObj.GetComponent<PCInputManager>();
        CameraInitPos = CameraObj.transform.localPosition;
        CameraInitRot = CameraObj.transform.localRotation;
        SpringArmLocalRot = Quaternion.identity;
    }

    void Update()
    {
        if (!PlayerInventory.activeSelf && !bIsMoveSuccess)
        {
            AttachtoTargetObj();
            CameraControll();
            SetAimOffset();
            Zoom();
        }
        else
        {
            InventoryActiveSet();
        }
    }

    void AttachtoTargetObj()
    {
        transform.position = TargetObj.transform.position;
    }

    void CameraControll()
    {
        Vector3 InputRotation = new Vector3(InputManager.lookDirection.y, InputManager.lookDirection.x, 0.0f);

        YawAxisValue += InputRotation.y * CameraRotationSpeed;
        PitchAxisValue += InputRotation.x * CameraRotationSpeed;

        PitchAxisValue = Mathf.Clamp(PitchAxisValue, -35.0f, 35.0f);

        CameraRotateDirection = new Vector3(PitchAxisValue, YawAxisValue, 0.0f);

        transform.rotation = Quaternion.Euler(CameraRotateDirection);
    }

    void SetAimOffset()
    {
        Ray ray = CameraObj.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0.0f));
        if (Physics.Raycast(ray, out RaycastHit hit, 200.0f, aimLayerMask))
        {
            AimTarget.transform.position = hit.point;
        }
        else
        {
            AimTarget.transform.position = ray.GetPoint(100.0f);
        }
    }

    void Zoom()
    {
        if (InputManager.bIsAim)
        {
            ZoomFactor = Mathf.MoveTowards(ZoomFactor, -1.5f, Time.deltaTime * ZoomSpeed);
        }
        else
        {
            ZoomFactor = Mathf.MoveTowards(ZoomFactor, -3.0f, Time.deltaTime * ZoomSpeed);
        }
        CameraObj.transform.localPosition = new Vector3(CameraObj.transform.localPosition.x, CameraObj.transform.localPosition.y, ZoomFactor);
    }

    void InventoryActiveSet()
    {
        
        if (PlayerInventory.activeSelf)
        {
            if (!bIsMoveSuccess)
            {
                SpringArmLocalRot = transform.localRotation;
            }
            bIsMoveSuccess = true;
            CameraObj.transform.localPosition = Vector3.MoveTowards(CameraObj.transform.localPosition, InventoryTargetTransform.localPosition, Time.deltaTime * CameraMoveSpeed);
            CameraObj.transform.localRotation = Quaternion.Slerp(CameraObj.transform.localRotation, InventoryTargetTransform.localRotation, Time.deltaTime * CameraMoveSpeed);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * CameraMoveSpeed);
        }
        else
        {
            CameraObj.transform.localPosition = Vector3.MoveTowards(CameraObj.transform.localPosition, CameraInitPos, Time.deltaTime * CameraMoveSpeed);
            CameraObj.transform.localRotation = Quaternion.Slerp(CameraObj.transform.localRotation, CameraInitRot, Time.deltaTime * CameraMoveSpeed);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, SpringArmLocalRot, Time.deltaTime * CameraMoveSpeed);
            if (CameraObj.transform.localPosition == CameraInitPos && CameraObj.transform.localRotation == CameraInitRot)
            {
                bIsMoveSuccess = false;
            }
        }
    }
}
