using UnityEngine;

public class BulletBurst : MonoBehaviour
{
    BoxCollider2D BurstCollider;
    float BulletHitFireLife;

    void Awake()
    {
        BurstCollider = GetComponent<BoxCollider2D>();
        BulletHitFireLife = 0.0f;
    }

    void Update()
    {
        BulletHitFireLife += Time.deltaTime;
        if (BulletHitFireLife > 0.3f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision)
        {
            if (collision.gameObject.layer == 11)
            {
                collision.gameObject.GetComponent<BossCharacter>().TakeDamage(2.5f);
            }
        }
    }
}
