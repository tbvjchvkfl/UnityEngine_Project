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
  > - 
<pre>
  <code>
    
  </code>
</pre>

