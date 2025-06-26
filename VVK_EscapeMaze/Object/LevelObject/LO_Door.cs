using System.Collections;
using UnityEngine;

public class LO_Door : MonoBehaviour
{
    public GameObject LeftDoor;
    public GameObject RightDoor;


    Vector3 LeftDoorInitPos;
    Vector3 RightDoorInitPos;
    Coroutine currentCoroutine;

    void Start()
    {
        LeftDoorInitPos = LeftDoor.transform.localPosition;
        RightDoorInitPos = RightDoor.transform.localPosition;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (currentCoroutine == null)
            {
                currentCoroutine = StartCoroutine(OnOpenDoor());
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
       if (other.gameObject.tag == "Player")
        {
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }
            currentCoroutine = StartCoroutine(OnCloseDoor());
        }
    }

    IEnumerator OnOpenDoor()
    {
        while (LeftDoor.transform.localPosition.x < 2.5f && RightDoor.transform.localPosition.x > -2.5f)
        {
            LeftDoor.transform.localPosition += new Vector3(2.0f * Time.deltaTime, 0.0f, 0.0f);
            RightDoor.transform.localPosition -= new Vector3(2.0f * Time.deltaTime, 0.0f, 0.0f);
            yield return null;
        }
        currentCoroutine = null;
    }

    IEnumerator OnCloseDoor()
    {
        while (LeftDoor.transform.localPosition.x > LeftDoorInitPos.x && RightDoor.transform.localPosition.x < RightDoorInitPos.x)
        {
            LeftDoor.transform.localPosition -= new Vector3(2.0f * Time.deltaTime, 0.0f, 0.0f);
            RightDoor.transform.localPosition += new Vector3(2.0f * Time.deltaTime, 0.0f, 0.0f);
            yield return null;
        }
        currentCoroutine = null;
    }

}
