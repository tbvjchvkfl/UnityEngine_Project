UI
-

Contents
-
- HUD (Singleton Pattern)
- Main Menu
- Pause Menu
- Goal Arrow UI

Class Diagram
-


Core Code
-
- ### HUD (Singleton Pattern)
  > - 게임 내 구현되어야하는 전체 UI를 관리할 HUD라는 클래스를 만들었고, 해당 스크립트 오브젝트의 Instance static으로 만들어 다른 UI 클래스들에서 전역으로 해당 클래스에 접근 할 수 있도록 구현했습니다.
  > - 게임이 구동되는 동안 씬이 전환되어도 각 씬 별 1개의 인스턴스만 존재할 수 있도록 DontDestroyOnLoad함수를 이용하여 해당 오브젝트는 삭제되지 않도록 구현했습니다.
  <pre>
   <code>
     
      public static HUD Instance { get; private set; }
            
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
     
   </code>
  </pre>
  > - 이 후, SceneManager.GetActiveScene().buildIndex를 이용하여 현재 씬을 확인하였고, GameObject.FindGameObjectWithTag 함수를 사용하여 각 씬 별로 표시되어야 하는 UI들을 찾아 세팅해주었습니다.
    <pre>
    <code>
      void InitUIObjects()
      {
          if (SceneManager.GetActiveScene().buildIndex == 0)
          {
              if (!MainMenu)
              {
                  if (MainMenu = GameObject.FindGameObjectWithTag("Main Menu"))
                  {
                      MainMenu.SetActive(true);
                  }
              }
          }
          if (SceneManager.GetActiveScene().buildIndex == 1)
          {
              if (!PauseMenu)
              {
                  if (PauseMenu = GameObject.FindGameObjectWithTag("Pause Menu"))
                  {
                      PauseMenu.SetActive(false);
                      bIsPause = false;
                  }
              }
                                                .
                                                .
                                                .
      }
    </code>
  </pre>

- ### Main Menu
  > - 게임 내 구현되어야하는 전체 UI를 관리할 HUD라는 클래스를 만들었고, 해당 스크립트 오브젝트의 Instance static으로 만들어 다른 UI 클래스들에서 전역으로 해당 클래스에 접근 할 수 있도록 구현했습니다.
  > - 게임이 구동되는 동안 씬이 전환되어도 각 씬 별 1개의 인스턴스만 존재할 수 있도록 DontDestroyOnLoad함수를 이용하여 해당 오브젝트는 삭제되지 않도록 구현했습니다.
  <pre>
   <code>
     
      public static HUD Instance { get; private set; }
            
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
     
   </code>
  </pre>

- ### Pause Menu
  > - 게임 내 구현되어야하는 전체 UI를 관리할 HUD라는 클래스를 만들었고, 해당 스크립트 오브젝트의 Instance static으로 만들어 다른 UI 클래스들에서 전역으로 해당 클래스에 접근 할 수 있도록 구현했습니다.
  > - 게임이 구동되는 동안 씬이 전환되어도 각 씬 별 1개의 인스턴스만 존재할 수 있도록 DontDestroyOnLoad함수를 이용하여 해당 오브젝트는 삭제되지 않도록 구현했습니다.
  <pre>
   <code>
     
      public static HUD Instance { get; private set; }
            
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
     
   </code>
  </pre>

  - ### Goal Arrow UI
  > - 게임 내 구현되어야하는 전체 UI를 관리할 HUD라는 클래스를 만들었고, 해당 스크립트 오브젝트의 Instance static으로 만들어 다른 UI 클래스들에서 전역으로 해당 클래스에 접근 할 수 있도록 구현했습니다.
  > - 게임이 구동되는 동안 씬이 전환되어도 각 씬 별 1개의 인스턴스만 존재할 수 있도록 DontDestroyOnLoad함수를 이용하여 해당 오브젝트는 삭제되지 않도록 구현했습니다.
  <pre>
   <code>
     
      public static HUD Instance { get; private set; }
            
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
     
   </code>
  </pre>
