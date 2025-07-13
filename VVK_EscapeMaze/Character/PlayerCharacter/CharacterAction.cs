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

    [Header("Animation Component")]
    public AnimationClip AimFire_Anim;
    public AnimationClip PistolAction_Anim_0;
    public AnimationClip Dodge_Anim;
    public AnimationClip Step_Anim;


    [Header("VFX Component")]
    public GameObject MuzzleFlash;

    [Header("Bullet Component")]
    public GameObject PistolBullet;

    [Header("Component")]
    public GameObject TargetAim;

    // BulletPool Component
    public BulletPool bulletPool { get; private set; }

    // Component
    PCInputManager inputManager;
    CharacterAnimation characterAnimation;
    
    // WeaponCooldown
    bool bIsAimFireCoolDown;

    // Data
    bool bIsDatasReady = false;

    public void InitEssentialData()
    {
        inputManager = GetComponent<PCInputManager>();
        characterAnimation = GetComponentInChildren<CharacterAnimation>();

        bulletPool = new BulletPool();
        bulletPool.InitBulletPool(PistolBullet);

        inputManager.OnNormalAttackEvent += ShootPistol;
        inputManager.OnDodgeEvent += Dodge;

        bIsDatasReady = true;
    }

    void Update()
    {
        if (bIsDatasReady)
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
        if (inputManager.bIsAim && inputManager.bIsNormalAttack)
        {
            if(!bIsAimFireCoolDown)
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
                    StartCoroutine(PistolShootInterval());
                }
                Debug.DrawRay(PistolMuzzle.transform.position, (TargetAim.transform.position - PistolMuzzle.transform.position).normalized * 20.0f, Color.red, 10.0f);
            }
        }
        else if(inputManager.bIsNormalAttack)
        {
            Debug.Log("Standing Action");
        }
    }

    IEnumerator PistolShootInterval()
    {
        yield return new WaitForSeconds(AimFire_Anim.length);
        bIsAimFireCoolDown = false;
    }

    public void ShowMuzzleFlashInAnim()
    {

    }

    void Dodge()
    {

        StartCoroutine(DodgetoStanding());
    }

    IEnumerator DodgetoStanding()
    {
        if (inputManager.bIsAim)
        {
            yield return new WaitForSeconds(Step_Anim.length);
        }
        else
        {
            yield return new WaitForSeconds(Dodge_Anim.length);
        }
    }

    private void OnDestroy()
    {
        inputManager.OnNormalAttackEvent -= ShootPistol;
    }
}
