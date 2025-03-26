using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CanonController : MonoBehaviour
{
    public GameObject CannonTop;
    public GameObject PlayerCharacter;
    public GameObject CanonBullet;
    public GameObject MuzzleFlash;
    public float RotationSpeed;

    [HideInInspector] public bool bIsControlled;

    PlayerInfo CharacterInfo;
    int BulletNum;
    float ShootingPower;

    void Awake()
    {
        CharacterInfo = PlayerCharacter.GetComponent<PlayerInfo>();
        MuzzleFlash.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    void Update()
    {
        SetEssentialData();
        SetCanonInput();
        FireCanon();
    }

    public void OnPossesController()
    {
        bIsControlled = true;
        BulletNum = CharacterInfo.MaxHP;
    }

    public void OnUnPossesController()
    {
        bIsControlled = false;
    }

    void SetEssentialData()
    {
        
    }

    void SetCanonInput()
    {
        if (bIsControlled)
        {
            if (gameObject.name == "Canon Bottom_L")
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.UpArrow))
                {
                    CannonTop.transform.Rotate(0.0f, 0.0f, Time.deltaTime * RotationSpeed);
                    if (CannonTop.transform.eulerAngles.z <= 180.0f)
                    {
                        CannonTop.transform.rotation = Quaternion.identity;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.DownArrow))
                {
                    CannonTop.transform.Rotate(0.0f, 0.0f, Time.deltaTime * RotationSpeed * -1.0f);
                    if (CannonTop.transform.eulerAngles.z <= 180.0f)
                    {
                        CannonTop.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                    }
                }
            }
            else if (gameObject.name == "Canon Bottom_R")
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.UpArrow))
                {
                    CannonTop.transform.Rotate(0.0f, 0.0f, Time.deltaTime * RotationSpeed * -1.0f);
                    if (CannonTop.transform.eulerAngles.z >= 180.0f)
                    {
                        CannonTop.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    }
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.DownArrow))
                {
                    CannonTop.transform.Rotate(0.0f, 0.0f, Time.deltaTime * RotationSpeed);
                    if (CannonTop.transform.eulerAngles.z >= 180.0f)
                    {
                        CannonTop.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
                    }
                }
            }

        }
    }

    void FireCanon()
    {
        if (bIsControlled)
        {
            if (Input.GetKey(KeyCode.X))
            {
                
            }
            else if (Input.GetKeyUp(KeyCode.X))
            {
                if (BulletNum > 0)
                {
                    SetMuzzleFlashVisibilty();
                    GameObject Bullet = Instantiate(CanonBullet);
                    Bullet.transform.position = MuzzleFlash.transform.position;
                    Bullet.GetComponent<Bullet>().BulletFire(CannonTop.transform.up, ShootingPower);
                }
            }
        }
    }

    void SetMuzzleFlashVisibilty()
    {
        MuzzleFlash.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Invoke("InvisibleMuzzleFlash", 0.3f);
    }

    void InvisibleMuzzleFlash()
    {
        MuzzleFlash.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }
}
