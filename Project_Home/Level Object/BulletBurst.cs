using UnityEngine;

public class BulletBurst : MonoBehaviour
{
    BoxCollider2D BurstCollider;

    void Awake()
    {
        BurstCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision)
        {
            if (collision.gameObject.layer == 11)
            {
                collision.gameObject.GetComponent<BossCharacter>().TakeDamage(2.5f);
            }
            Invoke("DestoryObj", 0.3f);
        }
    }

    void DestoryObj()
    {
        Destroy(this.gameObject);
    }
}
