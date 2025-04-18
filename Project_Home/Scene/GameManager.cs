using System.IO;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public bool FirstHeartObj;
    public bool SecondHeartObj;
    public bool ThirdHeartObj;

    public Vector2 PlayerLoc;
    public int PlayerHP;
}

public class GameManager : MonoBehaviour
{
    GameObject PlayerCharacter;
    GameObject EnemyCharacter;

    GameObject FObj;
    GameObject SObj;
    GameObject TObj;

    public static GameManager Instance{ get; private set; }

    public PlayerInfo PCInfo
    {
        get { return PlayerCharacter.GetComponent<PlayerInfo>(); }
    }
    public PlayerInput PCInput
    {
        get { return PlayerCharacter.GetComponent<PlayerInput>(); }
    }
    
    public bool bIsGameOver { get; private set; }
    public bool bIsBossBattle {  get; private set; }

    // SaveData
    public Vector2 PlayerLocation { get; private set; }
    public int PlayerHP { get; private set; }
    public int StageNumber { get; private set; }
    public bool bIsFHeartObject {  get; set; }
    public bool bIsSHeartObject { get; set; }
    public bool bIsTHeartObject { get; set; }
    
    public string SavePath {  get; private set; }

    void Awake()
    {
        SavePath = Path.Combine(Application.persistentDataPath, "SaveData.json");
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

    void Start()
    {
        LoadGame();
        DestroyHeartObj();
    }

    void Update()
    {
        InitEssentialData();
    }

    void InitEssentialData()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (!PlayerCharacter)
            {
                PlayerCharacter = GameObject.FindGameObjectWithTag("Player");
                FObj = GameObject.FindGameObjectWithTag("FHObj");
                SObj = GameObject.FindGameObjectWithTag("SHObj");
                TObj = GameObject.FindGameObjectWithTag("THObj");
                StageNumber = 1;
                bIsGameOver = false;
                LoadGame();
                DestroyHeartObj();
            }
        }
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (!PlayerCharacter)
            {
                PlayerCharacter = GameObject.FindGameObjectWithTag("Player");
                StageNumber = 2;
                bIsGameOver = false;
            }
            if (!EnemyCharacter)
            {
                EnemyCharacter = GameObject.FindGameObjectWithTag("Enemy");
                bIsBossBattle = true;
            }
        }
    }

    public void ApplyPlayerHP()
    {
        bIsGameOver = false;
        PlayerHP++;
        HUD.Instance.PlayerHealthBar.ShowPlayerUI();
    }

    public void SetPlayerHP(int Value)
    {
        PlayerHP -= Value;
        if (PlayerHP <= 0)
        {
            bIsGameOver = true;
        }
        HUD.Instance.PlayerHealthBar.ShowPlayerUI();
    }

    void DestroyHeartObj()
    {
        if (bIsFHeartObject && FObj)
        {
            Destroy(FObj.gameObject);
        }
        if (bIsSHeartObject && SObj)
        {
            Destroy(SObj.gameObject);
        }
        if (bIsTHeartObject && TObj)
        {
            Destroy(TObj.gameObject);
        }
    }

    public void SaveGame()
    {
        SaveData Data = new SaveData
        {
            PlayerLoc = PlayerCharacter.transform.position,
            PlayerHP = PlayerHP,
            FirstHeartObj = bIsFHeartObject,
            SecondHeartObj = bIsSHeartObject,
            ThirdHeartObj = bIsTHeartObject
        };

        string JSONSave = JsonUtility.ToJson(Data, true);
        File.WriteAllText(SavePath, JSONSave);
    }

    public void LoadGame()
    {
        if (File.Exists(SavePath))
        {
            string JSONSave = File.ReadAllText(SavePath);
            SaveData Data = JsonUtility.FromJson<SaveData>(JSONSave);

            PlayerLocation = Data.PlayerLoc;
            PlayerHP = Data.PlayerHP;
            bIsFHeartObject = Data.FirstHeartObj;
            bIsSHeartObject = Data.SecondHeartObj;
            bIsTHeartObject = Data.ThirdHeartObj;
            PCInfo.GetGameManagerData();
        }
        else
        {
            PlayerHP = 0;
            bIsFHeartObject = false;
            bIsSHeartObject = false;
            bIsTHeartObject = false;
        }
    }
}