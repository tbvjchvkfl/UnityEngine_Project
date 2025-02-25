using System.Collections.Generic;
using UnityEngine;

public class LightningPool : MonoBehaviour
{
    public int PoolSize;
    public GameObject PoolObject;
    public List<GameObject> LightningList;

    private void Awake()
    {
        InitPool();
    }

    public void InitPool()
    {
        LightningList.Clear();
        LightningList = new List<GameObject>();
        for (int i = 0; i < PoolSize; i++)
        {
            if(PoolObject)
            {
                GameObject lightning = GameObject.Instantiate(PoolObject);
                lightning.SetActive(false);
                lightning.GetComponent<Lightning>().InitializePool(this);

                LightningList.Add(lightning);
            }
        }
    }

    public GameObject UsingPool()
    {
        if (LightningList.Count != 0)
        {
            GameObject ReturnValue = LightningList[LightningList.Count - 1];
            ReturnValue.SetActive(true);
            LightningList.RemoveAt(LightningList.Count - 1);
            return ReturnValue;
        }
        return null;
    }

    public void ReturnPool(GameObject SpellObject)
    {
        if (SpellObject)
        {
            SpellObject.SetActive(false);
            LightningList.Add(SpellObject);
        }
        else
        {
            Debug.Log("Object is NULL!!!!!!");
        }
    }

    public void NullCheck()
    {
        foreach (GameObject obj in LightningList)
        {
            if (obj == null)
            {
                Debug.Log("Null");
            }
        }
    }
}
