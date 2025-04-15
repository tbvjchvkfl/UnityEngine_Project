UI
-

Contents
-
- HUD (Singleton Pattern)
- Goal Arrow UI
- Main Menu
- Pause Menu

클래스 구조도
-


핵심 코드
-
- ### HUD (Singleton Pattern)
  > - 게임 내 구현되어야하는 전체 UI를 관리할 HUD라는 클래스를 만들었고, 해당 스크립트 오브젝트의 Instance를 static으로 만들어 다른 UI 클래스들에서 전역으로 해당 클래스에 접근 할 수 있도록 구현했습니다.
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

- ### Goal Arrow UI
  > - 캐릭터가 획득해야하는 오브젝트의 위치를 알려주는 UI입니다.
  > - 씬에 존재하는 오브젝트들을 가져와서 캐릭터가 획득해야하는 순서의 역순으로 Stack에 넣어주었습니다.
  > - 오브젝트의 위치에서 현재 UI의 위치를 빼서 오브젝트의 위치를 가리키는 방향 벡터를 구했고, Mathf.Atan2를 이용하여 UI 오브젝트의 Y축 벡터와 오브젝트의 위치 사이의 라디안을 구했습니다.
  > - Mathf.Atan2함수를 사용해서 얻은 라디안에 Mathf.Rad2Deg를 곱해 라디안을 각도로 변환해 주었고, 이를 UI 오브젝트의 rotation에 적용해주었습니다.
  > - 캐릭터가 획득해야하는 오브젝트는 캐릭터와 Trigger되면 Destroy되게 해주었는데, 해당 오브젝트가 Destroy되서 null이 되면 Stack.Pop()을 해주어 획득해야하는 오브젝트의 순서대로 UI가 표시될 수 있도록 했습니다.
  <pre>
   <code>
      public GameObject FGoalObj;
      public GameObject SGoalObj;
      public GameObject TGoalObj;
      public GameObject LGoalObj;
      Stack<GameObject> GoalObjects;
           
      void InitGoalObjects()
      {
          GoalObjects.Push(LGoalObj);
          GoalObjects.Push(TGoalObj);
          GoalObjects.Push(SGoalObj);
          GoalObjects.Push(FGoalObj);
      }
      
      void CheckGoalObjectLocation()
      {
          if (GoalObjects.Peek())
          {
              Vector2 TargetDirection = GoalObjects.Peek().transform.position - GoalUI.transform.position;
              float TargetDegree = Mathf.Atan2(TargetDirection.x, TargetDirection.y) * Mathf.Rad2Deg;
              GoalUI.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -1.0f * TargetDegree);
          }
          else
          {
              GoalObjects.Pop();
          }
      }
   </code>
  </pre>

- ### Main Menu
  > - 플레이어의 Input에 따라 발생하는 포커스 전환 연출을 Coroutine을 사용해서 구현했습니다.
  > - 메인 메뉴에서 다른 씬으로 전환되었다가 다시 메인 메뉴로 돌아왔을 때에도 코루틴이 정상적으로 동작할 수 있도록 CheckActiveCoroutine 함수를 만들어 확인 후 실행되게 하는 알고리즘을 구현했습니다.
  <pre>
   <code>
     
      Coroutine CurrentCoroutine;
     
      void CheckActiveCoroutine(Coroutine SelectCoroutine)
      {
          if (CurrentCoroutine != null)
          {
              StopCoroutine(CurrentCoroutine);
          }
          CurrentCoroutine = SelectCoroutine;
      }
     
      void Update()
      {
          if (Input.GetKeyDown(KeyCode.LeftArrow))
          {
              if (!bIsRotation && !bIsGameStart)
              {
                  if (EventSystem.current.currentSelectedGameObject == GameStartBtn.gameObject)
                  {
                      EventSystem.current.SetSelectedGameObject(null);
                      bIsRotation = true;
                      CheckActiveCoroutine(StartCoroutine(RotateGametoTuto()));
                  }
                  if (EventSystem.current.currentSelectedGameObject == ExitBtn.gameObject)
                  {
                      EventSystem.current.SetSelectedGameObject(null);
                      bIsRotation = true;
                      CheckActiveCoroutine(StartCoroutine(RotateExittoGame()));
                  }
              }
          }
                                                 .
                                                 .
                                                 .
      }
     
      IEnumerator RotateGametoTuto()
      {
          while (bIsRotation)
          {
              yield return null;
              TitleText.color -= new Color(0.0f, 0.0f, 0.0f, Time.fixedDeltaTime * 2.0f);
              if (TitleText.color.a <= 0.0f)
              {
                  TitleText.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
              }
              SetTutorialMenu();
              RotationSpeed += 3.0f;
              RotationSpeed = Mathf.Clamp(RotationSpeed, 0.0f, 200.0f);
              RotationObject.transform.Rotate(0.0f, 0.0f, -Time.fixedDeltaTime * RotationSpeed);
              RotationSubObject.transform.Rotate(0.0f, 0.0f, -Time.fixedDeltaTime * RotationSpeed);
              if (RotationObject.transform.eulerAngles.z <= 295.0f && RotationObject.transform.eulerAngles.z > 65.0f)
              {
                  bIsRotation = false;
                  RotationSpeed = 0.0f;
                  RotationObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 295.0f);
                  RotationSubObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 295.0f);
                  TutorialBtn.interactable = true;
                  GameStartBtn.interactable = false;
                  EventSystem.current.SetSelectedGameObject(TutorialBtn.gameObject);
              }
          }
      }
   </code>
  </pre>

- ### Pause Menu
  - #### Graphic Menu ( 화면 해상도 )
    > - 화면 해상도를 조절하는 메뉴를 만들기 위해 List와 Dictionnary를 사용했습니다.
    > - Screen.resolutions에서 특정 해상도만 체크하여 List에 담았고, 이를 <int, Resolution> 형태로 묶어 Dictionary에 저장했습니다.
  <pre>
   <code>
        List<Resolution> Resolutions;
        Dictionary<int, Resolution> ResolutionDict;
        int ResolutionSettingIndex;
        
        void InitScreenResolution()
        {
            Resolutions = new List<Resolution>();
            ResolutionDict = new Dictionary<int, Resolution>();
            
            foreach (Resolution resol in Screen.resolutions)
            {
                if (resol.width == 3840 && resol.height == 2160 || 
                    resol.width == 2560 && resol.height == 1440 || 
                    resol.width == 1920 && resol.height == 1080 || 
                    resol.width == 1280 && resol.height == 720)
                {
                    Resolutions.Add(resol);
                }
            }
        
            for (int i = 0; i < Resolutions.Count; i++)
            {
                ResolutionDict.Add(i, Resolutions[i]);
            }
        
            for (int i = 0; i < ResolutionDict.Count; i++)
            {
                if (ResolutionDict[i].width == Screen.currentResolution.width)
                {
                    ResolutionSettingIndex = i;
                    ResolutionSettingText.text = $"{ResolutionDict[i]}";
                    break;
                }
            }
        }
   </code>
  </pre>
    > - 이 후, Player의 Input에 따라 게임 중 화면에 적용되어야할 화면 해상도 값과 관련 UI 요소들의 값을 변경해주었습니다.
  <pre>
   <code>
        void ChangedResolutionSetting()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ResolutionSettingIndex++;
                ResolutionSettingIndex = Mathf.Clamp(ResolutionSettingIndex, 0, ResolutionDict.Count - 1);
                ResolutionSettingText.text = $"{ResolutionDict[ResolutionSettingIndex]}";
                bIsMenuActive = true;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ResolutionSettingIndex--;
                ResolutionSettingIndex = Mathf.Clamp(ResolutionSettingIndex, 0, ResolutionDict.Count - 1);
                ResolutionSettingText.text = $"{ResolutionDict[ResolutionSettingIndex]}";
                bIsMenuActive = true;
            }
            if (Input.GetKeyDown(KeyCode.Return) && bIsMenuActive)
            {
                Screen.SetResolution(ResolutionDict[ResolutionSettingIndex].width, ResolutionDict[ResolutionSettingIndex].height, false);
                bIsResolutionSet = false;
                bIsMenuActive = false;
            }
        }
   </code>
  </pre>

- #### Graphic Menu ( 화면 크기 )
    > - 화면 크기 메뉴는 Tuple을 사용해서 구현했습니다.
    > - 메뉴 UI를 구현할 때 필요한 정보들을 <현재 화면 해상도를 변경 할 수 있는지에 대한 여부, 화면 모드, 화면 모드의 이름> 으로 묶어 List에 저장했고, List의 Index를 ScreenModeSettingIndex에 저장하여 List의 각 Index에 전역 접근이 용이하도록 했습니다.
  <pre>
   <code>
        List<Tuple<bool, FullScreenMode, string>> ScreenModes;
        int ScreenModeSettingIndex;

        void InitScreenMode()
        {
            ScreenModeSettingIndex = 2;
            ScreenModes = new List<Tuple<bool, FullScreenMode, string>>();
            ScreenModes.Add(new Tuple<bool, FullScreenMode, string>(true, FullScreenMode.Windowed, "Windowed"));
            ScreenModes.Add(new Tuple<bool, FullScreenMode, string>(false, FullScreenMode.FullScreenWindow, "FullScreenWindow"));
            ScreenModes.Add(new Tuple<bool, FullScreenMode, string>(false, FullScreenMode.ExclusiveFullScreen, "FullScreen"));
            bIsEnableChangeResolutionMode = ScreenModes[ScreenModeSettingIndex].Item1;
            Screen.fullScreenMode = ScreenModes[ScreenModeSettingIndex].Item2;
            ScreenModeSettingText.text = ScreenModes[ScreenModeSettingIndex].Item3;
        }
   </code>
  </pre>
    > - 이 후, Player의 Input에 따라 ScreenModeSettingIndex의 값을 변경해주었고, ScreenModes[ScreenModeSettingIndex]의 Item들을 관련 UI 항목들에 적용시켜주었습니다.
  <pre>
   <code>
        void ChangedModeSetting()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ScreenModeSettingIndex++;
                ScreenModeSettingIndex = Mathf.Clamp(ScreenModeSettingIndex, 0, 2);
                ScreenModeSettingText.text = ScreenModes[ScreenModeSettingIndex].Item3;
                bIsMenuActive = true;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ScreenModeSettingIndex--;
                ScreenModeSettingIndex = Mathf.Clamp(ScreenModeSettingIndex, 0, 2);
                ScreenModeSettingText.text = ScreenModes[ScreenModeSettingIndex].Item3;
                bIsMenuActive = true;
            }
            if (Input.GetKeyDown(KeyCode.Return) && bIsMenuActive)
            {
                Screen.fullScreenMode = ScreenModes[ScreenModeSettingIndex].Item2;
                bIsScreenMode = false;
                bIsMenuActive = false;
            }
        }
   </code>
  </pre>

  
  - #### Sound Menu
    > - 싱글톤 패턴으로 사용할 SoundManager 클래스를 만들어 게임 내 재생되어야할 AudioSource들의 Volume을 조절 할 수 있도록 구현했습니다.
    > - 이 후, Update 함수에서 OnFocusBackGroundVolumeSlider, OnFocusSoundEffectVolumeSlider 등의 함수를 호출하여 메뉴 항목의 Slider 값이 변화함에 따라 UI 값들이 표시될 수 있게 구현했습니다.
    > - 실질적인 음량 조절은 Slider 컴포넌트에 Callback 함수들을 바인딩 하여 Slider의 값을 SoundManager에서 관리하는 각 AudioSource들의 Volume에 적용시켜주었습니다.
  <pre>
   <code>
         ============ Sound Manager ============
     
      public void SetSoundEffectValue(float value)
      {
          PlayerWalk.volume = value;
          PlayerJump.volume = value;
          PlayerLand.volume = value;
      
          EnemyWalk.volume = value;
          EnemyPunchAttack.volume = value;
          EnemyKick.volume = value;
          EnemySkill.volume = value;
      
          CanonControll.volume = value;
          CanonShoot.volume = value;
          BulletBurst.volume = value;
          EarthQuake_0.volume = value;
          EarthQuake_1.volume = value;
          Elevator.volume = value;
      }
      
      public void SetBackGroundValue(float value)
      {
          BackGroundMusic.volume = value;
      }
     
         ============ Sound Menu ============    
     
      void OnChangedBackGroundVolumeValue(float value)
      {
          SoundManager.Instance.SetBackGroundValue(value);
      }
      
      void OnChangedSoundEffectVolumeValue(float value)
      {
          SoundManager.Instance.SetSoundEffectValue(value);
      }
     
      void OnFocusBackGroundVolumeSlider()
      {
          if (EventSystem.current.currentSelectedGameObject == BackGroundVolume.gameObject)
          {
              BackGroundSubjectVolumeText.color = Color.white;
              BackGroundVolumeText.text = $"{Mathf.Round(BackGroundVolume.value * 100.0f)}";
          }
          else
          {
              BackGroundSubjectVolumeText.color = Color.gray;
          }
      }
      
      void OnFocusSoundEffectVolumeSlider()
      {
          if (EventSystem.current.currentSelectedGameObject == SoundEffectVolume.gameObject)
          {
              SoundEffectSubjectVolumeText.color = Color.white;
              SoundEffectVolumeText.text = $"{Mathf.Round(SoundEffectVolume.value * 100.0f)}";
          }
          else
          {
              SoundEffectSubjectVolumeText.color = Color.gray;
          }
      }
   </code>
  </pre>



