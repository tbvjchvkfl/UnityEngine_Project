using UnityEngine;

public class GravityControl : MonoBehaviour
{
    public BoxCollider2D GravityWeightTargetSpace;
    void Start()
    {
        
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
                Debug.Log("Done");
            }
        }
    }
}
