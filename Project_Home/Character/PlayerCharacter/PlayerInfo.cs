using UnityEngine;

public class PlayerInfo : MonoBehaviour
{

    // ====================================
    //          - Public Data-
    // ====================================

    // Player Basic Data
    [Header("Basic Data")]
    [HideInInspector] public int CurrentHP;
    [HideInInspector] public int CurrentStageCount;
    public int MaxHP;
    

    

    // ====================================
    //          - Private Data-
    // ====================================

    void Awake()
    {
        CurrentHP = MaxHP;
    }

    void Start()
    {
        
    }

    void Update()
    {
    }

    public void TakeDamage(int Damage)
    {
        CurrentHP -= Damage;
    }
}
