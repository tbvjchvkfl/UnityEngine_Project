using UnityEngine;

public class Lightning : MonoBehaviour
{
    public AnimationClip clip;
    public Transform lightningPosition;
    public GameObject lightningTarget;
    private Player player;
    public GameObject TargetCharacter;

    private void Awake()
    {

    }

    void Start()
    {
        Destroy(lightningTarget, clip.length);
    }

    void Update()
    {
        
    }
}
