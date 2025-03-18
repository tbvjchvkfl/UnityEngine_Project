using UnityEngine;

public class GravityObjMovement : MonoBehaviour
{
    public GameObject GravityObj;
    public GameObject PlayerCharacter;
    
    public float TargetLocWeightValue;
    public float CollisionActiveValue;

    Transform CharacterTransform;
    Transform ObjTransform;
    Vector3 TargetLocation;
    Vector3 OriginLocation;
    PolygonCollider2D ObjCollider;
    SpriteRenderer ObjSprite;
    bool bIsInverseGravity;
    float LerpSpeed;
    

    void Awake()
    {
        ObjTransform = GetComponent<Transform>();
        ObjCollider = GetComponent<PolygonCollider2D>();
        ObjSprite = GetComponent<SpriteRenderer>();
        CharacterTransform = PlayerCharacter.GetComponent<Transform>();
        
        OriginLocation = ObjTransform.position;
        TargetLocation = new Vector3(ObjTransform.position.x, ObjTransform.position.y + TargetLocWeightValue, ObjTransform.position.z);
    }

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
}
