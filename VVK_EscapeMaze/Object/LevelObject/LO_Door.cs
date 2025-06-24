using System.Collections;
using UnityEngine;

public class LO_Door : MonoBehaviour
{
    public GameObject DoorObj;

    void Awake()
    {
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(OnOpenDoor());
            
        }
    }

    IEnumerator OnOpenDoor()
    {
        while (DoorObj.transform.position.y < 10.0f)
        {
            DoorObj.transform.position = Vector3.up * 2.0f * Time.deltaTime;
        }
        
        yield return null;
    }
}
