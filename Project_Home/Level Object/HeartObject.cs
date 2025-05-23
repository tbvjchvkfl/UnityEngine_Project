using UnityEngine;

public class HeartObject : MonoBehaviour
{
    [Header("Essential Data")]
    public float RotationSpeed;

    void Update()
    {
        transform.Rotate(new Vector3(0.0f, 0.0f, RotationSpeed * Time.deltaTime));
        if (transform.rotation.z >= 360.0f)
        {
            transform.rotation = Quaternion.identity;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            if (gameObject.tag == "FHObj")
            {
                GameManager.Instance.bIsFHeartObject = true;
            }
            else if (gameObject.tag == "SHObj")
            {
                GameManager.Instance.bIsSHeartObject = true;
            }
            else if (gameObject.tag == "THObj")
            {
                GameManager.Instance.bIsTHeartObject = true;
            }
            GameManager.Instance.ApplyPlayerHP();
            GameManager.Instance.SaveGame();
            Destroy(gameObject);
        }
    }
}
