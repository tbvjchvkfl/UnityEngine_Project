using System.Collections;
using UnityEngine;

public class LinkingBridge : MonoBehaviour
{
    public GameObject LinkingObject;

    void Awake()
    {
        if (LinkingObject)
        {
            Debug.Log(LinkingObject.transform.localEulerAngles.x);
        }
    }

    public void StartLink()
    {
        StartCoroutine(OnLinkedObject());
    }

    IEnumerator OnLinkedObject()
    {
        while (LinkingObject.transform.localEulerAngles.x < 360.0f && LinkingObject.transform.localEulerAngles.x >= 270.0f)
        {
            LinkingObject.transform.Rotate(Time.deltaTime * 1.2f, 0.0f, 0.0f);
            yield return null;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

        }
    }
}
