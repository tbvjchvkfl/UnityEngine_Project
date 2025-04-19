Managers
-

Contents
-
### [1. Singleton Pattern](#singleton-pattern)
### [2. Save & Load With Json](#save--load-with-json)

핵심 코드
-

- ### Singleton Pattern
  > - 게임의 전체 로직을 관리할 GameManager클래스를 만들었고, 해당 스크립트 오브젝트의 Instance를 static으로 만들어 다른 클래스들에서 전역 접근 할 수 있도록 구현했습니다.
  > - 게임이 시작되면 GameManagerInstance에 자기 자신을 할당하고, DontDestroyOnLoad함수를 사용하여 해당 오브젝트는 씬 이동이 되어도 삭제되지 않게 하였습니다.
  > - 만약 다른 씬에 또 다른 GameManager가 존재한다면 해당 오브젝트를 삭제하여 게임 전체에서 하나의 클래스만 존재하도록 구현했습니다.

<pre>
  <code>
    public static GameManager Instance{ get; private set; }
    
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
        DontDestroyOnLoad(this.gameObject);
    }
  </code>
</pre>

- ### Save & Load With Json
  > - 저장 할 데이터 목록을 담은 SaveData라는 클래스를 만들고, 메인 클래스에는 저장 후 로드한 데이터를 저장할 각 변수들을 만들었습니다.
  > - Awake 함수에서 Path.Combine함수를 사용하여 Unity에서 지정된 저장 경로에 JSON파일을 만들도록 하였고, 이를 string에 저장했습니다.
<pre>
  <code>
    ============== 저장 데이터 항목이 있는 서브 클래스 ==============
    
      [System.Serializable]
      public class SaveData
      {
          public bool FirstHeartObj;
          public bool SecondHeartObj;
          public bool ThirdHeartObj;
      
          public Vector2 PlayerLoc;
          public int PlayerHP;
      }
    
    ============== GameManager 메인 클래스 ==============
    
      public class GameManager : MonoBehaviour
      {
          // SaveData
          public Vector2 PlayerLocation { get; private set; }
          public int PlayerHP { get; private set; }
          public bool bIsFHeartObject {  get; set; }
          public bool bIsSHeartObject { get; set; }
          public bool bIsTHeartObject { get; set; }
          
          public string SavePath {  get; private set; }
      
          void Awake()
          {
              SavePath = Path.Combine(Application.persistentDataPath, "SaveData.json");
          }
      }
  </code>
</pre>
  > - SaveGame이라는 함수를 public으로 만들어 게임 로직 중 어디서든 게임을 저장 할 수 있도록 해주었습니다.
  > - 해당 함수에서 SaveData 서브클래스를 인스턴스화 해주었고, 클래스 멤버에 값을 할당했습니다.
  > - JsonUtility.ToJson함수를 사용하여 SaveData의 인스턴스를 JSON 문자열로 변환하였고, File.WriteAllText함수를 사용하여 위에서 만든 저장 경로에 JSON문자열을 써주었습니다.
<pre>
  <code>
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
  </code>
</pre>
  > - 저장 데이터를 로드할 때에도 역시 public으로 만들어 전역 접근이 가능하게 구현하였습니다.
  > - 데이터를 로드할 때에는 File.Exists함수를 사용하여 저장 경로에 Awake함수에서 SavePath에 저장했던 파일이 있는지 확인하였고, 저장 파일이 없다면 저장되어야하는 데이터에 기본값을 넣어주었습니다.
  > - SavePath라는 저장 파일이 있다면 File.ReadAllText를 사용하여 JSON문자열을 읽어왔고, 이를 다시 클래스 인스턴스로 변환하여 인스턴스의 멤버들을 메인 클래스에 저장되어야하는 데이터에 넣어주었습니다.
<pre>
  <code>
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
  </code>
</pre>
