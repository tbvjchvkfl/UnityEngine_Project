using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Owning Component
    CharacterAction characterAction;
    Rigidbody bulletRigidBody;

    void Awake()
    {
    }

    public void InitBullet(GameObject Owner)
    {
        characterAction = Owner.GetComponent<CharacterAction>();
        bulletRigidBody = GetComponent<Rigidbody>();
    }

    void ResetRigidBody()
    {
        bulletRigidBody.linearVelocity = Vector3.zero;
        bulletRigidBody.angularVelocity = Vector3.zero;
        bulletRigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    public void Fire(Vector3 bulletDirection, float bulletSpeed)
    {
        ResetRigidBody();
        bulletRigidBody.AddForce(bulletDirection * bulletSpeed, ForceMode.Impulse);
        StartCoroutine(OnCountLifeTime());
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Ground")
        {
            characterAction.ReturnBulletPool(this.gameObject);
            ResetRigidBody();
        }
    }

    IEnumerator OnCountLifeTime()
    {
        yield return new WaitForSeconds(10.0f);
        if(this.gameObject != null && gameObject.activeSelf)
        {
            characterAction.ReturnBulletPool(this.gameObject);
            ResetRigidBody();
        }
    }
}
