using UnityEngine;
using UnityEngine.UI;

public class QuestObjectPuse : MonoBehaviour
{
    public GameObject SpawnPointObject;
    public GameObject Canvas;
    public GameObject PlayerCharacter;
    public Image InteractionUIImage;
    
    void Awake()
    {
        if (InteractionUIImage)
        {
            InteractionUIImage.color = new Color(1, 1, 1, 1);
        }
    }

    void Update()
    {
        if (Canvas)
        {
            Vector3 LookDirection = PlayerCharacter.transform.position - Canvas.transform.position;
            LookDirection.y = 0;
            Canvas.transform.rotation = Quaternion.LookRotation(LookDirection, Vector3.up);
        }
    }

    public void InteractionObject()
    {
        if (SpawnPointObject)
        {
            SpawnPointObject.GetComponent<SpawnPoint>().bIsDelayEnd = true;
            Debug.Log("Delay End");
        }
    }
}
