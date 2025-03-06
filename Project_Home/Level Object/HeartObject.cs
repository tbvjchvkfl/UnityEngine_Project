using UnityEngine;

public class HeartObject : MonoBehaviour
{
    public CircleCollider2D ObjCollision;

    void Awake()
    {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.layer == LayerMask.GetMask("Player"))
        {
            Debug.Log("Player");
        }
    }
}
