using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject HitFire;

    SpriteRenderer BulletSprite;
    Rigidbody2D BulletRigid;
    CircleCollider2D BulletCollider;

    void Awake()
    {
        BulletRigid = GetComponent<Rigidbody2D>();
        BulletSprite = GetComponent<SpriteRenderer>();
        BulletCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    public void BulletFire(Vector3 FireDirection, float FireForce)
    {
        BulletRigid.AddForce(FireDirection * FireForce, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 11)
        {
            collision.gameObject.GetComponent<BossCharacter>().TakeDamage(1.0f);
        }
        if (collision.gameObject.layer == 6)
        {
            BulletSprite.enabled = false;
            BulletCollider.isTrigger = false;
            GameObject BulletBurst = Instantiate(HitFire);
            BulletBurst.transform.position = new Vector3(transform.position.x, transform.position.y + 1.7f, transform.position.z);
            GameObject.Destroy(this.gameObject);
        }
    }
}
