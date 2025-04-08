using NUnit.Framework.Internal;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameObject PlayerCharacter;

    public static GameManager Instance{ get; private set; }

    public PlayerInfo PCInfo
    {
        get { return PlayerCharacter.GetComponent<PlayerInfo>(); }
    }
    public PlayerInput PCInput
    {
        get { return PlayerCharacter.GetComponent<PlayerInput>(); }
    }
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
        InitEssentialData();
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
    }

    void InitEssentialData()
    {
        if (!PlayerCharacter)
        {
            if (PlayerCharacter = GameObject.FindGameObjectWithTag("Player"))
            {
                Debug.Log("Find PC");
            }
        }
    }

    public void ApplyPlayerHP()
    {
        PlayerHP++;
        PCInfo.GetGameManagerData();
        HUD.Instance.PlayerHealthBar.ShowPlayerUI();
    }

    public void SetPlayerHP(int Value)
    {
        PlayerHP -= Value;
        HUD.Instance.PlayerHealthBar.ShowPlayerUI();
    }

    public int LoadPlayerHP()
    {
        return PlayerHP;
    }
}