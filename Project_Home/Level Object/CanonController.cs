using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CanonController : MonoBehaviour
{
    public GameObject CannonTop;
    public GameObject PlayerCharacter;
    public GameObject CanonBullet;
    public GameObject MuzzleFlash;
    public GameObject TrajectoryDot;
    public float RotationSpeed;
    public float PowerWeight;

    public GameObject InterHelpUI;
    public GameObject BulletUI;
    public TMP_Text BulletText;

    public int BulletNum { get; private set; }
    public bool bIsControlled { get; private set; }

    PlayerInfo CharacterInfo;
    PlayerInput CharacterInput;

    float ShootingPower;
    List<GameObject> TrajectoryDots;
    Vector3 BulletDirection;

    void Awake()
    {
        CharacterInfo = PlayerCharacter.GetComponent<PlayerInfo>();
        CharacterInput = PlayerCharacter.GetComponent<PlayerInput>();
        MuzzleFlash.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        TrajectoryDots = new List<GameObject>();
    }

    void Update()
    {
        SetEssentialData();
        SetCanonInput();
        FireCanon();
        SetConnonUI();
    }

    public void OnPossesController()
    {
        bIsControlled = true;
        BulletNum = CharacterInfo.CurrentHP;
    }

    public void OnUnPossesController()
    {
        bIsControlled = false;
        HideTrajectory();
    }

    void SetEssentialData()
    {
        BulletDirection = CannonTop.transform.up;
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

    void SetMuzzleFlashVisibilty()
    {
        MuzzleFlash.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Invoke("InvisibleMuzzleFlash", 0.3f);
    }

    void InvisibleMuzzleFlash()
    {
        MuzzleFlash.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    void SetConnonUI()
    {
        if (bIsControlled)
        {
            InterHelpUI.SetActive(false);
            BulletUI.SetActive(true);
        }
        else
        {
            InterHelpUI.SetActive(true);
            BulletUI.SetActive(false);
        }
        if (BulletNum > 0)
        {
            BulletText.text = $"{BulletNum}";
        }
        else
        {
            BulletText.text = "EMPTY";
        }
    }
}
