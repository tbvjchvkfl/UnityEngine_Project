using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public GameObject PlayerCharacter;

    public float RotationSpeed;
    public float PortalMaxSize;
    public float PortalMinSize;

    Transform PlayerTransform;

    void Awake()
    {
        PlayerTransform = PlayerCharacter.GetComponent<Transform>();
    }

    void Update()
    {
        CheckPlayerLocation();
        transform.Rotate(0.0f, 0.0f, Time.deltaTime * RotationSpeed);
    }

    void CheckPlayerLocation()
    {
        float PlayerDistance = Vector2.Distance(transform.position, PlayerTransform.position);
        if (PlayerDistance <= 5.0f)
        {
            transform.localScale += new Vector3(0.05f, 0.05f, 0.0f);
            if (transform.localScale.x >= PortalMaxSize)
            {
                transform.localScale = new Vector3(PortalMaxSize, PortalMaxSize, 1.0f);
            }
        }
        else
        {
            transform.localScale -= new Vector3(0.05f, 0.05f, 0.0f);
            if (transform.localScale.x <= PortalMinSize)
            {
                transform.localScale = new Vector3(PortalMinSize, PortalMinSize, 1.0f);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    public void MovetoNextStage()
    {
        SceneManager.LoadScene("Stage_1_Boss");
    }
}
