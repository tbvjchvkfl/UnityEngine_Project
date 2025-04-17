Level Object
-

Contents
-
### [1. Canon Object](#canon-object)
### [2. Gravity Object](#gravity-object)


핵심 코드
-
- ### Canon Object
  > - Player가 X키를 눌렀을때와 누르고 있을 때, X키 입력을 해제했을 때로 나누어 발사를 구현했습니다.
  > - 포탄이 날아갈 궤적 UI를 ShowTrajectory함수를 통해 List에 넣어주었습니다.
  > - X키를 누르고 있는 동안 List에 담겨있던 궤적 UI에 포물선 공식을 적용하여 포탄이 얼마만큼 날아갈 것인지를 구현했습니다.
  > - 누르고 있던 X키를 때면 HideTrajectory함수를 통해 궤적 UI를 삭제하고 List를 비워주었고, Bullet에 있는 BulletFire함수를 통해 Canon의 Y축 방향으로 AddForce함수를 호출하여 포탄이 발사되게 구현 했습니다.
<pre>
  <code>
      ============ CanonController.cs ============
      public float PowerWeight;
      float ShootingPower;
      List&lt;GameObject&gt; TrajectoryDots;
      Vector3 BulletDirection;
    
      void SetEssentialData()
      {
          BulletDirection = CannonTop.transform.up;
      }
    
      void FireCanon()
      {
          if (bIsControlled && BulletNum > 0)
          {
              if (Input.GetKeyDown(KeyCode.X))
              {
                  ShowTrajectory();
              }
              else if (Input.GetKey(KeyCode.X))
              {
                  UpdateTrajectory();
              }
              else if (Input.GetKeyUp(KeyCode.X))
              {
                  HideTrajectory();
                  SetMuzzleFlashVisibilty();
                  GameObject Bullet = Instantiate(CanonBullet);
                  Bullet.transform.position = MuzzleFlash.transform.position;
                  Bullet.GetComponent<Bullet>().BulletFire(BulletDirection, ShootingPower);
                  BulletNum--;
                  ShootingPower = 0.0f;
                  CharacterInfo.RemovePocketItem();
              }
          }
      }
      
      void ShowTrajectory()
      {
          for (int i = 0; i < 5; i++)
          {
              GameObject TrajecDot = Instantiate(TrajectoryDot);
              TrajecDot.transform.position = (BulletDirection * i) + MuzzleFlash.transform.position;
              TrajectoryDots.Add(TrajecDot);
          }
      }
      
      void UpdateTrajectory()
      {
          ShootingPower += Time.deltaTime * PowerWeight;
          ShootingPower = Mathf.Clamp(ShootingPower, 0, 10.0f);
      
          for (int i = 0; i < TrajectoryDots.Count; i++)
          {
              float DotInterval = i * 0.1f;
              float TrajectX = BulletDirection.x * ShootingPower * DotInterval;
              float TrajectY = (BulletDirection.y * ShootingPower * DotInterval) - (Physics2D.gravity.magnitude * DotInterval * DotInterval) / 2.0f;
              TrajectoryDots[i].transform.position = MuzzleFlash.transform.position + new Vector3(TrajectX, TrajectY, 0.0f);
          }
      }
      
      void HideTrajectory()
      {
          for (int i = 0; i < TrajectoryDots.Count; i++)
          {
              GameObject.Destroy(TrajectoryDots[i]);
          }
          TrajectoryDots.Clear();
      }

                              

      ============ Bullet.cs ============
            
      public void BulletFire(Vector3 FireDirection, float FireForce)
      {
          BulletRigid.AddForce(FireDirection * FireForce, ForceMode2D.Impulse);
      }
  </code>
</pre>

- ### Gravity Object
  > - Player가 해당 오브젝트와 상호작용하면 StartReverseGravity를 통해 특정 범위에 오버랩 되어있는 오브젝트들의 위치를 변경하는 ModifyGravityScale함수를 InvokeRepeating에 등록하여 호출했습니다.
  > - ModifyGravityScale함수에서는 PlayerCharacter의 회전, 중력 크기, 입력에 대한 이동 방향을 반전 시켜주었고, PlayerCharcter가 아닌 오브젝트들은 Lerp함수를 활용하여 이동 시작과 이동 끝에서 부드럽게 움직일 수 있도록 구현했습니다.
  > - 중력 반전에 의해 이동되는 Gravity Object들은 위치가 이동함에 따라 색상과 Tigger여부를 변경해주었습니다.
<pre>
  <code>
    =============== Gravity Controller.cs ===============
    public void StartReverseGravity()
    {
        Collider2D[] OverlapObjects = Physics2D.OverlapBoxAll(GravityWeightTargetSpace.bounds.center, GravityWeightTargetSpace.bounds.size, 0.0f);
        foreach(Collider2D obj in  OverlapObjects)
        {
            if (obj.gameObject.layer == 3)
            {
                TargetRigid = obj.gameObject.GetComponent<Rigidbody2D>();
                TargetTransform = obj.gameObject.GetComponent<Transform>();
                if (TargetRigid)
                {
                    bIsInverseGravity = true;
                    InvokeRepeating("ModifyGravityScale", 0.0f, Time.deltaTime);
                }
            }
        }
    }

    public void ReturnOriginGravity()
    {
        Collider2D[] OverlapObjects = Physics2D.OverlapBoxAll(GravityWeightTargetSpace.bounds.center, GravityWeightTargetSpace.bounds.size, 0.0f);
        foreach (Collider2D obj in OverlapObjects)
        {
            if (obj.gameObject.layer == 3)
            {
                TargetRigid = obj.gameObject.GetComponent<Rigidbody2D>();
                TargetTransform = obj.gameObject.GetComponent<Transform>();
                if (TargetRigid)
                {
                    bIsInverseGravity = false;
                    InvokeRepeating("ModifyGravityScale", 0.0f, Time.deltaTime);
                }
            }
        }
    }

    void ModifyGravityScale()
    {
        if (!bIsInverseGravity)
        {
            if (TargetRigid.gravityScale <= 0.0f)
            {
                TargetRigid.gravityScale += Time.deltaTime * 2.0f;
            }
            else
            {
                TargetRigid.gravityScale += Time.deltaTime * NewGravityScale;
                if (TargetRigid.gravityScale >= 0.1f && MainCamera.transform.eulerAngles.z >= 90.0f)
                {
                    TargetRigid.gravityScale = 0.1f;
                }
                if (TargetRigid.gravityScale < 0.7f)
                {
                    CameraRotationSpeed = Mathf.Lerp(CameraRotationSpeed, 50.0f, Time.deltaTime * 1.0f);
                }
                else
                {
                    CameraRotationSpeed = Mathf.Lerp(CameraRotationSpeed, 50.0f, Time.deltaTime * 0.15f);
                }
                MainCamera.transform.Rotate(0.0f, 0.0f, CameraRotationSpeed * Time.deltaTime * -1.0f);

                TargetTransform.Rotate(0.0f, 0.0f, CameraRotationSpeed * Time.deltaTime * -1.0f);

                if (TargetTransform.eulerAngles.z > 180.0f)
                {
                    TargetTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                }
                if (MainCamera.transform.eulerAngles.z > 180.0f)
                {
                    MainCamera.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                }
                if (TargetRigid.gravityScale >= 3.0f)
                {
                    TargetRigid.gravityScale = 3.0f;
                }
                if (TargetTransform.rotation == Quaternion.Euler(0.0f, 0.0f, 0.0f) && MainCamera.transform.rotation == Quaternion.Euler(0.0f, 0.0f, 0.0f) && TargetRigid.gravityScale == 3.0f)
                {
                    CancelInvoke();
                }
            }
        }
        else
        {
            if (TargetRigid.gravityScale >= 0.0f)
            {
                TargetRigid.gravityScale -= Time.deltaTime * 2.0f;
            }
            else
            {
                TargetRigid.gravityScale -= Time.deltaTime * NewGravityScale;
                if (TargetRigid.gravityScale <= -0.05f && MainCamera.transform.eulerAngles.z <= 90.0f)
                {
                    TargetRigid.gravityScale = -0.05f;
                }
                if (TargetRigid.gravityScale < -0.7f)
                {
                    CameraRotationSpeed = Mathf.Lerp(CameraRotationSpeed, 50.0f, Time.deltaTime * 1.0f);
                }
                else
                {
                    CameraRotationSpeed = Mathf.Lerp(CameraRotationSpeed, 50.0f, Time.deltaTime * 0.15f);
                }
                MainCamera.transform.Rotate(0.0f, 0.0f, CameraRotationSpeed * Time.deltaTime);

                TargetTransform.Rotate(0.0f, 0.0f, CameraRotationSpeed * Time.deltaTime);

                if (TargetTransform.eulerAngles.z >= 180.0f)
                {
                    TargetTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                    
                }
                if (MainCamera.transform.eulerAngles.z >= 180.0f)
                {
                    MainCamera.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                }
                if (TargetRigid.gravityScale <= -3.0f)
                {
                    TargetRigid.gravityScale = -3.0f;
                }
                if (TargetTransform.rotation == Quaternion.Euler(0.0f, 0.0f, 180.0f) && MainCamera.transform.rotation == Quaternion.Euler(0.0f, 0.0f, 180.0f) && TargetRigid.gravityScale == -3.0f)
                {
                    CancelInvoke();
                }
            }
        }
    }
                  
    =============== Gravity Object.cs ===============
    void Update()
    {
        bIsInverseGravity = GravityObj.GetComponent<GravityControl>().bIsInverseGravity;
        if (bIsInverseGravity)
        {
            if (CharacterTransform.eulerAngles.z < 90.0f)
            {
                LerpSpeed = 0.01f;
            }
            else
            {
                LerpSpeed = 0.5f;
            }
            ObjTransform.position = Vector3.Lerp(ObjTransform.position, TargetLocation, Time.deltaTime * LerpSpeed);
            
            if (ObjTransform.position.y >= CollisionActiveValue)
            {
                ObjSprite.color += new Color(0.003f, 0.003f, 0.003f, 0.0f);
                if (ObjSprite.color.r >= 1.0f)
                {
                    ObjSprite.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    ObjCollider.isTrigger = false;
                }
            }
        }
        else
        {
            if (CharacterTransform.eulerAngles.z > 90.0f)
            {
                LerpSpeed = 0.01f;
            }
            else
            {
                LerpSpeed = 0.5f;
            }
            ObjTransform.position = Vector3.Lerp(ObjTransform.position, OriginLocation, Time.deltaTime * LerpSpeed);
            if (ObjTransform.position.y <= OriginLocation.y + 5.0f)
            {
                ObjSprite.color -= new Color(0.003f, 0.003f, 0.003f, 0.0f);
                if (ObjSprite.color.r <= 0.6f)
                {
                    ObjSprite.color = new Color(0.6f, 0.6f, 0.6f, 1.0f);
                    ObjCollider.isTrigger = true;
                }
            }
        }
    }
  </code>
</pre>

