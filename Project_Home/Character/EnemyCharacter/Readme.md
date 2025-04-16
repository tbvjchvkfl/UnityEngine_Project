Boss Character
-

Contents
-
### [1. Strategy Pattern](#strategy-pattern)
### [2. Move State](#move-state)
### [3. Skill State](#skill-state)

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

- ### Move State
   > - PlayerCharcter와 BossCharacter의 위치를 비교해서 PlayerCharacter의 X축 값이 BossCharacter의 X축 값보다 작다면 왼쪽으로 그렇지 않다면 오른쪽으로 이동하게 구현했습니다.
   > - BossCharacter에 공격 가능 범위를 나타낼 BoxCollider를 추가하여 해당 범위에 PlayerCharacter가 들어와 있다면 ModifyingState함수를 통해 공격 상태로 전환해주었습니다.
   > - 공격 상태로 진입하기 전에는 Vector3.Distance함수를 사용해서 PlayerCharacter와 BossCharacter의 거리를 체크하여 어떤 공격 상태로 전환할 것인지를 결정하도록 구현했습니다.
<pre>
   <code>
         public class BossMoveState : IBossCharacterState
         {
             public void ExcuteState(BossCharacter boss)
             {
                 SetCharacterMovement(boss);
                 Collider2D HitEnemy = Physics2D.OverlapBox(boss.SkillAttackSpace.bounds.center, boss.SkillAttackSpace.bounds.size, 0.0f, LayerMask.GetMask("Player"));
                 if (HitEnemy)
                 {
                     &lt; TargetCharacter와 거리 체크 &gt;
      
                     float TargetDistance = Vector3.Distance(boss.TargetCharacter.transform.position, boss.transform.position);
                     if (TargetDistance < 2.0f)
                     {
                         if (boss.bIsKickAttack)
                         {
                             boss.ModifyingState(new BossAttackState());
                         }
                         else
                         {
                             boss.ModifyingState(new BossComboAttackState());
                         }
                     }
                     if (TargetDistance >= 2.0f)
                     {
                         if (boss.bIsSkillAttack)
                         {
                             boss.ModifyingState(new BossSkillState());
                         }
                     }
                 }
             }
                        
             &lt; BossCharacter의 방향 결정과 이동 함수 &gt;
                        
             void SetCharacterMovement(BossCharacter boss)
             {
                 if (boss.TargetCharacter.transform.position.x <= boss.transform.position.x)
                 {
                     boss.MovementDirection = Vector2.left;
                 }
                 else if (boss.TargetCharacter.transform.position.x > boss.transform.position.x)
                 {
                     boss.MovementDirection = Vector2.right;
                 }
                 boss.CharacterBody.linearVelocity = new Vector2(boss.MovementDirection.x * boss.MoveSpeed, boss.CharacterBody.linearVelocityY);
             }
         }
   </code>
</pre>   
- ### Skill State
  > - Skill State로 전환되었을 때, CheckTargetLoc함수를 통해 PlayerCharacter의 위치를 파악 후 해당 위치에 Skill Notice오브젝트를 생성하여 스킬을 사용했다라는 알림(경고)를 줄 수 있도록 구현했습니다.
  > - ExcuteDelayTime과 생성된 오브젝트의 색상 값을 SKill Attack의 실행 조건으로 지정하여 Player가 충분히 회피할 수 있도록 구현했고, SKillAttack이 실행 됨과 동시에 SkillNotice 오브젝트의 삭제와 관련 Coroutine을 실행해주었습니다.
  > - Skill Attack이 실행되면 함께 실행되는 Coroutine에서는 Player가 보스를 공격할 수 있도록 일정한 시간 만큼 대기하도록 구현했으며, 전투 지형 변화와 Camera Shaking 효과를 구현했습니다.
<pre>
   <code>
         public class BossSkillState : IBossCharacterState
         {
             GameObject SkillNotice;
             BossCharacter Boss;
             AnimationClip SlamptoIdle;
             Vector3 TargetLocation;
             bool bIsSlampReady;
             float AppearHeight;
             float ExcuteDelayTime;
             float SkillAfterExcuteDelayTime;
         
             public void EnterState(BossCharacter boss)
             {
                 SkillEssentialData(boss);
                 CheckTargetLoc(boss);
             }
         
             public void ExcuteState(BossCharacter boss)
             {
                 boss.AnimationController.SetBool("SlamReady", bIsSlampReady);
                 if (SkillNotice)
                 {
                     ExcuteDelayTime -= Time.deltaTime;
                     SkillNotice.GetComponent&lt;SpriteRenderer&gt;().color += new Color(0.0f, 0.0f, 0.0f, Time.deltaTime * 0.5f);
                     if (ExcuteDelayTime <= 0.0f && SkillNotice.GetComponent&lt;SpriteRenderer&gt;().color.a >= 1.0f)
                     {
                         
                         SkillNotice.GetComponent&lt;SpriteRenderer&gt;().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                         GameObject.Destroy(SkillNotice);
                         boss.transform.position = TargetLocation;
                         boss.AnimationController.SetTrigger("Skill Attack");
                         boss.StartCoroutine(SkillAttackAfterDelay());
                     }
                 }
             }
         
             public void ExitState(BossCharacter boss)
             {
                 boss.bIsSkillAttack = false;
                 boss.bIsCameraMoving = false;
                 boss.bIsPlatformMoving = false;
                 boss.DestoryCannonBullet();
             }

             &lt; Coroutine 함수 &gt;
             IEnumerator SkillAttackAfterDelay()
             {
                 Boss.bIsCameraMoving = true;
                 yield return new WaitForSeconds(0.5f);
         
                 Boss.bIsPlatformMoving = true;
                 yield return new WaitForSeconds(SkillAfterExcuteDelayTime);
         
                 bIsSlampReady = false;
                 Boss.AnimationController.SetBool("SlamReady", bIsSlampReady);
         
                 yield return new WaitForSeconds(SlamptoIdle.length);
                 Boss.ModifyingState(new BossIdleState());
             }
                            
             &lt; Skill State의 변수들 초기화 함수 &gt;
                        
             void SkillEssentialData(BossCharacter boss)
             {
                 Boss = boss;
                 SkillAfterExcuteDelayTime = boss.SkillAfterExcuteDelayTime;
                 SlamptoIdle = boss.SlamptoIdle;
                 bIsSlampReady = true;
                 ExcuteDelayTime = boss.SkillExcuteDelayTime;
                 AppearHeight = boss.AppearHeight;
                 boss.MovementDirection = Vector2.zero;
             }
                            
             &lt; PlayerCharacter 위치 파악 함수 &gt;
                        
             void CheckTargetLoc(BossCharacter boss)
             {
                 TargetLocation = new Vector3(boss.TargetCharacter.transform.position.x, boss.TargetCharacter.transform.position.y + AppearHeight, boss.TargetCharacter.transform.position.z);
                 SkillNotice = GameObject.Instantiate&lt;GameObject&gt;(boss.SkillNotice);
                 SkillNotice.transform.position = TargetLocation;
                 if (boss.transform.position.x < boss.TargetCharacter.transform.position.x)
                 {
                     SkillNotice.GetComponent&lt;SpriteRenderer&gt;().flipX = false;
                 }
                 else
                 {
                     SkillNotice.GetComponent&lt;SpriteRenderer&gt;().flipX = true;
                 }
             }
         }
   </code>
</pre>
