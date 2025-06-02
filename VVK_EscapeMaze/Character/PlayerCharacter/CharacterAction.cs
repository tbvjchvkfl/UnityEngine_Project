using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    [Header("Weapon Component")]
    public GameObject PistolGun;
    public GameObject PistolMuzzle;
    public float PistolBulletSpeed = 10.0f;
    public AnimationClip PistolFire_Anim;

    public GameObject ShotGun;
    public GameObject ShotGunMuzzle;
    public float ShotGunBulletSpeed = 10.0f;

    public GameObject RifleGun;
    public GameObject RifleMuzzle;
    public float RifleBulletSpeed = 10.0f;

    [Header("Bullet Component")]
    public GameObject PistolBullet;

    [Header("Component")]
    public GameObject TargetAim;
    public float BulletPoolSize = 100.0f;

    // Component
    PCInputManager inputManager;
    CharacterAnimation characterAnimation;

    // EssentialData
    List<GameObject> bulletPool;


    // WeaponCooldown
    bool bIsPistolFire;

    public bool bIsPistol {  get; private set; }
    public bool bIsShotGun {  get; private set; }
    public bool bIsRifle {  get; private set; }
    

    void Awake()
    {
        inputManager = GetComponent<PCInputManager>();
        characterAnimation = GetComponentInChildren<CharacterAnimation>();
    }

    void Start()
    {
        InitBulletPool();
    }

    void Update()
    {
        AppearPistolMesh();
    }

    void AppearPistolMesh()
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

    void InitBulletPool()
    {
        bulletPool = new List<GameObject>();
        if (PistolBullet)
        {
            for (int i = 0; i < BulletPoolSize; i++)
            {
                GameObject bullet = GameObject.Instantiate(PistolBullet);
                bullet.SetActive(false);
                bulletPool.Add(bullet);
            }
        }
    }

    GameObject UseBulletPool()
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

    public void ShootPistol()
    {
        if (inputManager.bIsAim && inputManager.bIsNormalAttack)
        {
            GameObject Bullet = UseBulletPool();
            Vector3 bulletDirection = (TargetAim.transform.position - PistolMuzzle.transform.position).normalized;
            if (Bullet && !bIsPistolFire)
            {
                bIsPistolFire = true;
                Bullet.SetActive(true);
                Bullet.transform.position = PistolMuzzle.transform.position;
                Bullet.GetComponent<Bullet>().Fire(bulletDirection, PistolBulletSpeed);
                StartCoroutine(PistolShootInterval());
            }
            else
            {
                Debug.Log("Empty Bullet");
            }
            Debug.DrawRay(PistolMuzzle.transform.position, (TargetAim.transform.position - PistolMuzzle.transform.position).normalized * 20.0f, Color.red, 10.0f);
        }
    }

    IEnumerator PistolShootInterval()
    {
        yield return new WaitForSeconds(PistolFire_Anim.length);
        bIsPistolFire = false;
    }
}
