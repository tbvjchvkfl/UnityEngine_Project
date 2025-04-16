Boss Character
-

클래스 구조도
-

핵심 코드
-

- ### Strategy Pattern
   > - 보스 캐릭터가 상태별로 동작할 수 있도록 상태 패턴을 구현했습니다.
   > - IBossCharacterState에 각 상태별 공용으로 사용할 함수들을 가상함수로 선언해주었고, 각 상태별 행동을 정의할 상태 클래스에서 가상함수들을 정의해주었습니다.
   > - 컨텍스트 클래스에서 AWake와 Update 에서 상태 클래스에 있는 가상 함수들을 호출해주었고, 컨텍스트 클래스에 ModifyingState함수를 통해 특정 조건에 맞게 상태 전환이 일어날 수 있도록 구현했습니다.
<pre>
  <code>
    &lt; 모든 상태 클래스가 공용으로 사용할 인터페이스 &gt;
    
      public interface IBossCharacterState
      {
          void EnterState(BossCharacter boss);
          void ExcuteState(BossCharacter boss);
          void ExitState(BossCharacter boss);
      }
    
    &lt; 위 인터페이스를 상속받은 상태 클래스 &gt;
    
      public class BossIdleState : IBossCharacterState
      {
          public void EnterState(BossCharacter boss)
          {
          }
      
          public void ExcuteState(BossCharacter boss)
          {
          }
      
          public void ExitState(BossCharacter boss)
          {
          }
      }

    &lt; 각 상태 클래스의 가상 함수 실행 및 공용 조건을 판단할 컨텍스트 클래스  &gt;
    
      public class BossCharacter : MonoBehaviour
      {
          IBossCharacterState BossState;
              
          void Awake()
          {
              BossState = new BossIdleState();
              BossState.EnterState(this);
          }
              
          void Update()
          {
              BossState.ExcuteState(this);
          }   
              
          public void ModifyingState(IBossCharacterState NewState)
          {
              if (BossState.ToString() == NewState.ToString())
              {
                  return;
              }
              BossState.ExitState(this);
              BossState = NewState;
              BossState.EnterState(this);
          }
      }
  </code>
</pre>
