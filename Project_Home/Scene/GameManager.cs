using NUnit.Framework.Internal;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int PlayerHP {  get; private set; }

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {

    }

    public int LoadPlayerHP()
    {
        return PlayerHP;
    }
}