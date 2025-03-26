using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D BulletRigid;
    void Awake()
    {
        BulletRigid = GetComponent<Rigidbody2D>();
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
}
