using System.Diagnostics.Tracing;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public AnimationClip clip;
    public GameObject lightningTarget;
    public GameObject TargetCharacter;

    public float knockbackPower;

    LightningPool pool;
    BoxCollider2D boxCollider;
    float DeActiveTime;

    private void Awake()
    {
        DeActiveTime = 0.0f;
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
    }

    void Update()
    {
        DeActiveTime += Time.deltaTime;
        if (DeActiveTime >= clip.length)
        {
            pool.ReturnPool(this.gameObject);
            DeActiveTime = 0.0f;
        }
    }

    public void InitializePool(LightningPool lightningpool)
    {
        pool = lightningpool;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameObject OverlapObj = collision.gameObject;
            if (OverlapObj)
            {
                Player player = OverlapObj.GetComponent<Player>();
                if(player)
                {
                    player.TakeDamage(10.0f, knockbackPower);
                }
            }
        }
    }
}
