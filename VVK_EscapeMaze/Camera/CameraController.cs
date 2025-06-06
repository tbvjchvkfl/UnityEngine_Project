using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    [Header("Camera Property")]
    public GameObject TargetObj;
    public GameObject CameraObj;
    public GameObject AimTarget;
    public float CameraRotationSpeed; // 1보다 높아지면 과하게 빨라짐 왠만하면 0~1값을 유지할 것
    public float ZoomSpeed = 3.0f;

    PCInputManager InputManager;
    
    Vector3 CameraRotateDirection;
    float YawAxisValue;
    float PitchAxisValue;
    float ZoomFactor = -3.0f;

    LayerMask aimLayerMask;

    void Awake()
    {
        InputManager = TargetObj.GetComponent<PCInputManager>();
    }

    void Update()
    {
        AttachtoTargetObj();
        CameraControll();
        SetAimOffset();
        Zoom();
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
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, aimLayerMask))
        {
            AimTarget.transform.position = hit.point;
        }
        else
        {
            AimTarget.transform.position = ray.GetPoint(10f);
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
}
