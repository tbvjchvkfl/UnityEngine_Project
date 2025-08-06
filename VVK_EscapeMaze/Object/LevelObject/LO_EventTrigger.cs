using UnityEngine;

public class LO_EventTrigger : MonoBehaviour
{
    public GameObject LinkedSpawnPoint;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (LinkedSpawnPoint)
            {
                LinkedSpawnPoint.GetComponent<SpawnPoint>().bIsDelayEnd = true;
                Destroy(this.gameObject);
            }
        }
    }
}
