using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LO_Cabinet : MonoBehaviour
{
    public GameObject CabinetDoor;
    public Image InteractionUIImg;

    void Awake()
    {
        if (InteractionUIImg)
        {
            InteractionUIImg.color = Color.clear;
        }
    }

    void Update()
    {
        
    }

    public void OpenDoor()
    {
        if (CabinetDoor)
        {
            StartCoroutine(OpentheDoor());
            CabinetDoor.GetComponent<BoxCollider>().isTrigger = true;
        }
    }

    IEnumerator OpentheDoor()
    {
        Debug.Log("Open");
        while (CabinetDoor.transform.localRotation.y > -90.0f)
        {
            CabinetDoor.transform.localRotation = Quaternion.Slerp(CabinetDoor.transform.localRotation, Quaternion.Euler(0, -90.0f, 0), Time.deltaTime * 2.5f);
            yield return null;
        }
        CabinetDoor.transform.localRotation = Quaternion.Euler(0, -90.0f, 0);
        CabinetDoor.GetComponent<BoxCollider>().isTrigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (InteractionUIImg)
            {
                InteractionUIImg.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (InteractionUIImg)
            {
                InteractionUIImg.color = Color.clear;
            }
        }
    }
}
