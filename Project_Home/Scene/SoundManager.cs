using NUnit.Framework;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    // BGM
    public AudioSource BackGroundMusic;

    // SFX
    public AudioSource PlayerWalk;
    public AudioSource PlayerJump;
    public AudioSource PlayerLand;

    public AudioSource EnemyWalk;
    public AudioSource EnemyPunchAttack;
    public AudioSource EnemyKick;
    public AudioSource EnemySkill;

    public AudioSource CanonControll;
    public AudioSource CanonShoot;
    public AudioSource BulletBurst;
    public AudioSource EarthQuake_0;
    public AudioSource EarthQuake_1;
    public AudioSource Elevator;


    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        
    }

    public void SetSoundEffectValue(float value)
    {
        PlayerWalk.volume = value;
        PlayerJump.volume = value;
        PlayerLand.volume = value;

        EnemyWalk.volume = value;
        EnemyPunchAttack.volume = value;
        EnemyKick.volume = value;
        EnemySkill.volume = value;

        CanonControll.volume = value;
        CanonShoot.volume = value;
        BulletBurst.volume = value;
        EarthQuake_0.volume = value;
        EarthQuake_1.volume = value;
        Elevator.volume = value;
    }

    public void SetBackGroundValue(float value)
    {
        BackGroundMusic.volume = value;
    }

    public void PlayWalkSound()
    {
        PlayerWalk.Play();
    }

    public void PlayJumpSound()
    {
        PlayerJump.Play();
    }

    public void PlayLandingSound()
    {
        PlayerLand.Play();
    }
}
