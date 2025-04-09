using UnityEngine;

public class SpawnBullet : MonoBehaviour
{
    void Awake()
    {
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                //collision.gameObject.GetComponent<PlayerInfo>().
            }
        }
    }

}
