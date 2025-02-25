using UnityEngine;
//using static UnityEngine.RuleTile.TilingRuleOutput;

public class CameraMovement : MonoBehaviour
{
    public Transform TargetTransform;
    public float CameraInterpValue;

    private void Awake()
    {
        transform.position = new Vector3(TargetTransform.position.x, TargetTransform.position.y, -10.0f);
        CameraInterpValue = 1.0f;
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, TargetTransform.position, Time.deltaTime * CameraInterpValue);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10.0f);
    }
}
