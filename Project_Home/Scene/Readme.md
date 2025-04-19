Managers
-

Contents
-
### [1. Singleton Pattern](#singleton-pattern)
### [2. Save & Load With Json](#save--load-with-json)

핵심 코드
-

- ### Singleton Pattern
  > - 
<pre>
  <code>
    public static GameManager Instance{ get; private set; }
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
  </code>
</pre>

- ### Save & Load With Json
  > - 
<pre>
  <code>
    
  </code>
</pre>

