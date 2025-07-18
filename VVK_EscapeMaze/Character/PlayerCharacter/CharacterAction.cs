using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool
{
    // Pool Size
    public int BulletPoolSize { get; private set; } = 100;

    // Pool List
    List<GameObject> bulletPool;

    public void InitBulletPool(GameObject spawnBullet)
    {
        bulletPool = new List<GameObject>();

        if (spawnBullet)
        {
            for (int i = 0; i < BulletPoolSize; i++)
            {
                GameObject bullet = GameObject.Instantiate(spawnBullet);
                bullet.SetActive(false);
                bulletPool.Add(bullet);
            }
        }
    }

    public GameObject UseBulletPool()
    {
        if (bulletPool.Count > 0)
        {
            GameObject useBullet = bulletPool[bulletPool.Count - 1];
            bulletPool.RemoveAt(bulletPool.Count - 1);
            return useBullet;
        }
        return null;
    }

    public void ReturnBulletPool(GameObject returnObject)
    {
        if (returnObject)
        {
            returnObject.SetActive(false);
            bulletPool.Add(returnObject);
        }
    }
}

public class CharacterAction : MonoBehaviour
{
    [Header("Weapon Component")]
    public GameObject PistolGun;
    public GameObject PistolMuzzle;
    public float PistolBulletSpeed = 10.0f;

    public GameObject ShotGun;
    public GameObject ShotGunMuzzle;
    public float ShotGunBulletSpeed = 10.0f;

    public GameObject RifleGun;
    public GameObject RifleMuzzle;
    public float RifleGunBulletSpeed = 10.0f;

    [Header("VFX Component")]
    public GameObject MuzzleFlash;

    [Header("Bullet Component")]
    public GameObject PistolBullet;
    public GameObject ShotGunBullet;

    [Header("Component")]
    public GameObject TargetAim;

    // BulletPool Component
    public BulletPool bulletPool { get; private set; }

    // Component
    PCInputManager inputManager;
    CharacterAnimation characterAnimation;
    CharacterMovement characterMovement;
    PlayerInventory playerInventory;

    Coroutine DodgeCoroutine;
    

    // Skill Data
    public SkillNode[] SkillList { get; private set; } = new SkillNode[2];
    public float SkillIndex { get; set; } = 0.0f;
    public bool bIsSkillActivate { get; set; } = false;

    // WeaponCooldown
    bool bIsAimFireCoolDown;

    // Data
    bool bIsDataReady = false;
    

    // Delegate
    public delegate void OnEquipSkillSlot_LDelegate(SkillNode nodeData, int Num);
    public event OnEquipSkillSlot_LDelegate OnEquipSkillSlot;


    public void InitEssentialData()
    {
        inputManager = GetComponentInParent<PCInputManager>();
        characterMovement = GetComponentInParent<CharacterMovement>();
        playerInventory = GetComponentInParent<PlayerInventory>();
        characterAnimation = GetComponent<CharacterAnimation>();

        bulletPool = new BulletPool();
        bulletPool.InitBulletPool(PistolBullet);

        playerInventory.OnEquipSkillEvent += EquipSkill;
        inputManager.OnNormalAttackEvent += ShootPistol;
        inputManager.OnSpecialSkillEvent += SpecialSkill;
        inputManager.OnDodgeEvent += Dodge;

        bIsDataReady = true;
    }

    void Update()
    {
        if (bIsDataReady)
        {
            AttachPistolMesh();
        }
    }

    void AttachPistolMesh()
    {
        if (inputManager.bIsAim)
        {
            PistolGun.SetActive(true);
        }
        else if(characterAnimation.AimStateIndex >= 0.0f)
        {
            PistolGun.SetActive(false);
        }
    }

   void ShootPistol()
    {
        if (!bIsSkillActivate)
        {
            if (!inputManager.bIsAim && !inputManager.bIsSkillReady)
            {
                Debug.Log("Standing Action");
            }
            else if (!inputManager.bIsAim && inputManager.bIsSkillReady && SkillList[0])
            {
                Debug.Log("Normal Skill Activated");
                SkillIndex = SkillList[0].SkillData.SkillIndex;
                bIsSkillActivate = true;
            }
        }
    }

    public void BeginShootBulletInAnim()
    {
        if (inputManager.bIsAim && !bIsAimFireCoolDown)
        {
            GameObject bullet = bulletPool.UseBulletPool();
            if (bullet)
            {
                bIsAimFireCoolDown = true;
                Vector3 bulletDirection = (TargetAim.transform.position - PistolMuzzle.transform.position).normalized;
                Bullet bulletComponent = bullet.GetComponent<Bullet>();
                bulletComponent.InitBullet(this.gameObject);
                bullet.SetActive(true);
                bullet.transform.position = PistolMuzzle.transform.position;
                bulletComponent.Fire(bulletDirection, PistolBulletSpeed);
                ShowMuzzleFlashInAnim();
            }
            Debug.DrawRay(PistolMuzzle.transform.position, (TargetAim.transform.position - PistolMuzzle.transform.position).normalized * 20.0f, Color.red, 10.0f);
        }
    }

    public void EndShootBulletInAnim()
    {
        bIsAimFireCoolDown = false;
    }

    public void ShowMuzzleFlashInAnim()
    {

    }

    void SpecialSkill()
    {
        if(inputManager.bIsSkillReady && SkillList[1] && !bIsSkillActivate)
        {
            Debug.Log("Special Skill Activated");
            SkillIndex = SkillList[1].SkillData.SkillIndex;
            bIsSkillActivate = true;
        }
    }

    void EquipSkill(SkillNode node, int nodeNum)
    {
        if (node)
        {
            SkillList[nodeNum] = node;
            OnEquipSkillSlot?.Invoke(node, nodeNum);
        }
    }

    void Dodge()
    {
        if (DodgeCoroutine == null)
        {
            characterAnimation.SetDodgeDirection();
            DodgeCoroutine = StartCoroutine(DodgetoStanding());
        }
    }

    IEnumerator DodgetoStanding()
    {
        characterMovement.StepAndDodgeMovement(inputManager.inputDirection, 0.5f);
        yield return new WaitForSeconds(0.3f);

        if (inputManager.bIsDodge)
        {
            characterMovement.StepAndDodgeMovement(inputManager.inputDirection, 1.0f);
            yield return new WaitForSeconds(0.5f);
        }

        inputManager.bIsStep = false;
        inputManager.bIsDodge = false;
        StopCoroutine(DodgeCoroutine);
        DodgeCoroutine = null;
    }

    private void OnDestroy()
    {
        inputManager.OnNormalAttackEvent -= ShootPistol;
        inputManager.OnDodgeEvent -= Dodge;
    }
}
