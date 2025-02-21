using System.Diagnostics.Tracing;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public AnimationClip clip;
    public GameObject lightningTarget;
    public GameObject TargetCharacter;

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
            //Collider2D hit = Physics2D.OverlapBox(boxCollider.bounds.center, boxCollider.bounds.size, 0.0f);
            TargetCharacter.GetComponent<Player>().TakeDamage(10.0f);
            Debug.Log("Hit");
        }
    }
}
