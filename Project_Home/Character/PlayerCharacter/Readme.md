Player Character
-

목차
-
### [1. Moving Action](#moving-action)
### [2.Dash(Roll)](#dash(roll))
### [3.Sliding](#sliding)
### [4. Interaction](#interaction)


클래스 구조도
-


핵심 코드
-
- ### Moving Action
  - #### Move
    > - PlayerInputSystem을 사용하여 필요한 입력을 설정했고, 파라미터로 InputValue를 받아 입력에 의한 Vector를 MovementDirerction에 저장해주었습니다.
    > - SetMovementDirection 함수에서 MovementDirerction.x에 의해 캐릭터의 방향을 계산했고, SpriteRender 컴포넌트에 flipX값을 조절하여 캐릭터가 방향 전환을 할 수 있게 구현했습니다.

![Image](https://github.com/user-attachments/assets/9d3cddfb-ee53-428f-bd2c-7cbdbf478c2b)

<pre>
  <code>
      public void OnMove(InputValue inputValue)
      {
          if (HUD.Instance.bIsPause)
          {
              MovementDirection = Vector2.zero;
              return;
          }
          if (!bIsRolling && !bIsPowerJump && !bIsSliding && !bIsDuringGravity && !bIsHit && !bIsDeath && !bIsStun && !bIsCanonControll)
          {
              MovementDirection = inputValue.Get<Vector2>();
      
              if (bIsInteraction)
              {
                  if (MovementDirection.x < 0)
                  {
                      AnimationController.SetBool("Pulling", true);
                      AnimationController.SetBool("Pushing", false);
                  }
                  else if (MovementDirection.x > 0)
                  {
                      AnimationController.SetBool("Pulling", false);
                      AnimationController.SetBool("Pushing", true);
                  }
                  else
                  {
                      AnimationController.SetBool("Pulling", false);
                      AnimationController.SetBool("Pushing", false);
                  }
              }
          }
      }
      void SetMovingDirection()
      {
          if (!HUD.Instance.bIsPause && !GameManager.Instance.bIsGameOver)
          {
              if (bIsHit)
              {
                  MovementDirection = Vector3.zero;
                  AnimationController.SetBool("Moving", false);
              }
              else if (bIsInteraction)
              {
                  CharacterSprite.flipX = false;
              }
              else
              {
                  if (MovementDirection.x < 0)
                  {
                      CharacterSprite.flipX = true;
                  }
                  if (MovementDirection.x > 0)
                  {
                      CharacterSprite.flipX = false;
                  }
              }
          }
      }
  </code>
</pre>

- ### Dash(Roll)
  > - 이동(Move)와 마찬가지로 PlayerInputSystem을 통해 입력을 받았고, Coroutine에서 PlayerCharacter의 SpriteRender.flipX 값에 따라 정해진 방향으로 AddForce함수를 호출하여 특정 방향으로 순간적인 이동을 할 수 있게 구현했습니다.
  > - DoDash 코루틴이 실행될 때 Physics2D.IgnoreLayerCollision을 사용하여 임의의 시간 동안 특정 오브젝트와는 충돌하지 않게 해주었습니다.
    
<pre>
  <code>
      public void OnDash()
      {
          if (!bIsInAir && !bIsSliding && !bIsHit && !bIsDeath && !bIsStun && !bIsCanonControll)
          {
              if (bIsInteraction)
              {
                  bIsInteraction = false;
              }
              StartCoroutine(DoDash());
          }
      }
          
      IEnumerator DoDash()
      {
          AnimationController.SetTrigger("Rolling");
          CharacterBody.linearVelocity = Vector2.zero;
          bIsRolling = true;
          float OriginGravityScale = CharacterBody.gravityScale;
          CharacterBody.gravityScale = 1.0f;
          Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("EnemyCharacter"), true);
          MovementDirection = Vector2.zero;
          AnimationController.SetBool("Moving", false);
          if (CharacterSprite.flipX)
          {
              CharacterBody.AddForce(Vector2.left * DashPower, ForceMode2D.Impulse);
          }
          else
          {
              CharacterBody.AddForce(Vector2.right * DashPower, ForceMode2D.Impulse);
          }
      
          yield return new WaitForSeconds(RollingAnimation.length - 0.09f);
      
          Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("EnemyCharacter"), false);
          bIsRolling = false;
          CharacterBody.gravityScale = OriginGravityScale;
      }
  </code>
</pre>

- Sliding
  > - PlayerCharacter에 바닥 체크용 BoxCollider2D를 추가하고 해당 위치에서 RunWay라는 이름을 가진 레이어를 체크하는 방식으로 Sliding을 구현했습니다.
  > - BoxCollider2D 범위에 조건에 맞는 오브젝트가 Trigger되었다면 해당 오브젝트 Z축 회전값을 가져와서 PlayerCharacter의 회전 각도와 내려가야하는 방향을 정해주었고, Sliding 애니메이션을 출력하였습니다.
<pre>
  <code>
      void CheckSliding()
      {
          Collider2D CheckPlatform = Physics2D.OverlapBox(CheckingFloor.bounds.center, CheckingFloor.bounds.size, 0.0f, LayerMask.GetMask("RunWay"));
          if (CheckPlatform)
          {
              bIsSliding = true;
              MovementDirection = Vector2.zero;
              float TargetRotValue = CheckPlatform.gameObject.GetComponent<Transform>().eulerAngles.z;
              transform.rotation = Quaternion.Euler(0.0f, 0.0f, TargetRotValue);
              if (TargetRotValue <= 180.0f)
              {
                  CharacterSprite.flipX = true;
              }
              else
              {
                  CharacterSprite.flipX = false;
              }
          }
          else
          {
              if (bIsInverseGravity || CharacterBody.gravityScale != 3.0f)
              {
                  return;
              }
              bIsSliding = false;
              transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
          }
          AnimationController.SetBool("Sliding", bIsSliding);
      }
  </code>
</pre>

- ### Interaction
  > - Player가 특정 오브젝트와 상호작용 하기 위해 Vector2(1, 0) 방향과 Vector2(-1, 0) 방향으로 Raycast를 만들어주었습니다.
  > - MovementDirection에 의해 정해지는 PlayerCharacter의 SpriteRender컴포넌트에 flipX값에 따라 Raycast를 InteractTrace에 저장해주었습니다.
  > - 이 후, Interaction 가능한 오브젝트들에 tag를 설정해 주었고, Raycast에 닿은 오브젝트의 tag에 따라 각각의 로직을 실행시켜주었습니다.
<pre>
  <code>
      void StartTrace()
      {
          RaycastHit2D R_Trace = Physics2D.Raycast(CharacterCapsule.bounds.center, Vector2.right, CharacterCapsule.bounds.size.x / 2, LayerMask.GetMask("Interactable"));
          RaycastHit2D L_Trace = Physics2D.Raycast(CharacterCapsule.bounds.center, Vector2.left, CharacterCapsule.bounds.size.x / 2, LayerMask.GetMask("Interactable"));
          if (!CharacterSprite.flipX)
          {
              InteractTrace = R_Trace;
          }
          else
          {
              InteractTrace = L_Trace;
          }
      }
  </code>
</pre>
<pre>
  <code>
      public void OnInteraction()
      {
          StartTrace();
          if (InteractTrace)
          {
              if (InteractTrace.collider.gameObject.tag == "PullingObj")
              {
                  if (bIsInteraction)
                  {
                      bIsInteraction = false;
                      AnimationController.SetBool("Interaction", bIsInteraction);
                      InteractionObj.GetComponent<PlatformControl>().bIsInteracting = false;
                      InteractionObj = null;
                  }
                  else
                  {
                      bIsInteraction = true;
                      AnimationController.SetBool("Interaction", bIsInteraction);
                      InteractionObj = InteractTrace.collider.gameObject;
                      InteractionObj.GetComponent<PlatformControl>().bIsInteracting = true;
                  }
              }
                                              .
                                              .
                                              .
          }
      }
  </code>
</pre>
