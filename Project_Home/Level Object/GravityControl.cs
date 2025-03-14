using Unity.VisualScripting;
using UnityEngine;

public class GravityControl : MonoBehaviour
{
    public BoxCollider2D GravityWeightTargetSpace;
    public float NewGravityScale;
    public float CameraRotationSpeed;
    public GameObject CameraObject;

    [HideInInspector] public bool bIsInverseGravity;

    Rigidbody2D TargetRigid;
    Transform TargetTransform;
    Camera MainCamera;
    CameraMovement MainCameraMove;

    void Awake()
    {
        MainCamera = CameraObject.GetComponent<Camera>();
        MainCameraMove = CameraObject.GetComponent<CameraMovement>();
    }

    void Update()
    {
        
    }

    public void StartReverseGravity()
    {
        Collider2D[] OverlapObjects = Physics2D.OverlapBoxAll(GravityWeightTargetSpace.bounds.center, GravityWeightTargetSpace.bounds.size, 0.0f);
        foreach(Collider2D obj in  OverlapObjects)
        {
            // 레이어가 플레이어거나 GravityPlatform이면 여기서 중력값을 조절해줌
            // 리지드 바디를 사용할지 그저 축 이동을 할지 생각해 볼것.
            if (obj.gameObject.layer == 3)
            {
                TargetRigid = obj.gameObject.GetComponent<PlayerInput>().GetPlayerRigid();
                TargetTransform = obj.gameObject.GetComponent<Transform>();
                if (TargetRigid)
                {
                    bIsInverseGravity = true;
                    InvokeRepeating("ModifyGravityScale", 0.0f, Time.deltaTime);
                }
            }
        }
    }

    void ModifyGravityScale()
    {
        if (!bIsInverseGravity)
        {
            if (TargetRigid.gravityScale <= 0.0f)
            {
                TargetRigid.gravityScale += Time.deltaTime * 2.0f;
            }
            else
            {
                TargetRigid.gravityScale += Time.deltaTime * NewGravityScale;
                if (TargetRigid.gravityScale >= 0.1f && MainCamera.transform.eulerAngles.z >= 90.0f)
                {
                    TargetRigid.gravityScale = 0.1f;
                }
                if (TargetRigid.gravityScale < 0.7f)
                {
                    CameraRotationSpeed = Mathf.Lerp(CameraRotationSpeed, 50.0f, Time.deltaTime * 1.0f);
                }
                else
                {
                    CameraRotationSpeed = Mathf.Lerp(CameraRotationSpeed, 50.0f, Time.deltaTime * 0.15f);
                }
                MainCamera.transform.Rotate(0.0f, 0.0f, CameraRotationSpeed * Time.deltaTime * -1.0f);

                TargetTransform.Rotate(0.0f, 0.0f, CameraRotationSpeed * Time.deltaTime * -1.0f);

                if (TargetTransform.eulerAngles.z <= 0.0f)
                {
                    TargetTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    CancelInvoke();
                }
                if (MainCamera.transform.eulerAngles.z <= 0.0f)
                {
                    MainCamera.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                }
                if (TargetRigid.gravityScale >= 3.0f)
                {
                    TargetRigid.gravityScale = 3.0f;
                }
            }
        }
        else
        {
            if (TargetRigid.gravityScale >= 0.0f)
            {
                TargetRigid.gravityScale -= Time.deltaTime * 2.0f;
            }
            else
            {
                TargetRigid.gravityScale -= Time.deltaTime * NewGravityScale;
                if (TargetRigid.gravityScale <= -0.1f && MainCamera.transform.eulerAngles.z <= 90.0f)
                {
                    TargetRigid.gravityScale = -0.1f;
                }
                if (TargetRigid.gravityScale < -0.7f)
                {
                    CameraRotationSpeed = Mathf.Lerp(CameraRotationSpeed, 50.0f, Time.deltaTime * 1.0f);
                }
                else
                {
                    CameraRotationSpeed = Mathf.Lerp(CameraRotationSpeed, 50.0f, Time.deltaTime * 0.15f);
                }
                MainCamera.transform.Rotate(0.0f, 0.0f, CameraRotationSpeed * Time.deltaTime);

                TargetTransform.Rotate(0.0f, 0.0f, CameraRotationSpeed * Time.deltaTime);

                if (TargetTransform.eulerAngles.z >= 180.0f)
                {
                    TargetTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                    CancelInvoke();
                }
                if (MainCamera.transform.eulerAngles.z >= 180.0f)
                {
                    MainCamera.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                }
                if (TargetRigid.gravityScale <= -3.0f)
                {
                    TargetRigid.gravityScale = -3.0f;
                }
            }
        }
    }

    public void ReturnOriginGravity()
    {
        Collider2D[] OverlapObjects = Physics2D.OverlapBoxAll(GravityWeightTargetSpace.bounds.center, GravityWeightTargetSpace.bounds.size, 0.0f);
        foreach (Collider2D obj in OverlapObjects)
        {
            // 레이어가 플레이어거나 GravityPlatform이면 여기서 중력값을 조절해줌
            // 리지드 바디를 사용할지 그저 축 이동을 할지 생각해 볼것.
            if (obj.gameObject.layer == 3)
            {
                TargetRigid = obj.gameObject.GetComponent<PlayerInput>().GetPlayerRigid();
                TargetTransform = obj.gameObject.GetComponent<Transform>();
                if (TargetRigid)
                {
                    bIsInverseGravity = false;
                    InvokeRepeating("ModifyGravityScale", 0.0f, Time.deltaTime);
                }
            }
        }
    }
}
