using UnityEngine;
using UnityEngine.UI;

public class HPGuage : MonoBehaviour
{
    public RectTransform BaseImage;
    public RectTransform HealthGuageImage;
    public float RotateSpeed;

    public Animator animator;
    public RuntimeAnimatorController runanimcontroller;

    [HideInInspector] public bool StopRotation;
    

    void Awake()
    {
        RotateSpeed = 80.0f;
        StopRotation = false;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (!StopRotation)
        {
            BaseImage.Rotate(0.0f, 0.0f, RotateSpeed * Time.deltaTime);
            HealthGuageImage.Rotate(0.0f, 0.0f, RotateSpeed * Time.deltaTime);
            if (BaseImage.rotation.z >= 360.0f && HealthGuageImage.rotation.z >= 360.0f)
            {
                BaseImage.rotation = Quaternion.identity;
                HealthGuageImage.rotation = Quaternion.identity;
            }
        }
        else
        {
            animator.runtimeAnimatorController = runanimcontroller;
        }
    }
}
