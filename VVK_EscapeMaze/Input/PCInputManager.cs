using UnityEngine;
using UnityEngine.InputSystem;

public class PCInputManager : MonoBehaviour
{
    public Vector2 inputDirection {  get; private set; }
    public Vector2 lookDirection { get; private set; }
    public bool bIsJump {  get; private set; }
    public bool bIsSprint {  get; private set; }
    public bool bIsWalk {  get; private set; }
    public bool bIsCrouch { get; private set; }
    public bool bIsAim { get; private set; }
    public bool bIsNormalAttack {  get; private set; }
    public bool bIsLockOn {  get; private set; }
    public bool bIsInteraction {  get; private set; }

    // Non-Control
    public bool bIsHit { get; set; } = false;

    // Delegates and Events
    public delegate void OnNormalAttackDelegate();
    public event OnNormalAttackDelegate OnNormalAttackEvent;

    public delegate void OnInteractionDelegate();
    public event OnInteractionDelegate OnInteractionEvent;

    public delegate void OnInventoryDelegate();
    public event OnInventoryDelegate OnInventoryEvent;

    public delegate void OnLockOnDelegate();
    public event OnLockOnDelegate OnLockOnEvent;

    public delegate void OnSkillDelegate();
    public event OnSkillDelegate OnSkillEvent;

    void Awake()
    {
        
    }

    public void OnMove(InputValue inputValue)
    {
        if (!bIsHit)
        {
            inputDirection = inputValue.Get<Vector2>();
        }
    }

    public void OnLook(InputValue inputValue)
    {
        lookDirection = inputValue.Get<Vector2>();
    }

    public void OnSprint(InputValue inputValue)
    {
        if (!bIsHit)
        {
            bIsSprint = inputValue.isPressed;
        }
    }

    public void OnChangeMoveState(InputValue inputValue)
    {
        if (bIsWalk)
        {
            bIsWalk = false;
        }
        else
        {
            bIsWalk = true;
        }
    }

    public void OnCrouch()
    {
        if (!bIsHit)
        {
            if (bIsCrouch)
            {
                bIsCrouch = false;
            }
            else
            {
                bIsCrouch = true;
            }
        }
    }

    public void OnAim(InputValue inputValue)
    {
        if (bIsHit)
        {
            bIsAim = false;
        }
        else
        {
            bIsAim = inputValue.isPressed;
        }
    }

    public void OnAttack(InputValue inputValue)
    {
        if (!bIsHit)
        {
            bIsNormalAttack = inputValue.isPressed;
            if (bIsNormalAttack)
            {
                OnNormalAttackEvent?.Invoke();
            }
        }
    }

    public void OnSkill()
    {
        if (!bIsHit)
        {
            OnSkillEvent?.Invoke();
        }
    }

    public void OnLockOn()
    {
        if(bIsLockOn)
        {
            bIsLockOn = false;
        }
        else
        {
            bIsLockOn = true;
        }
        OnLockOnEvent?.Invoke();
    }

    public void OnInteraction(InputValue inputValue)
    {
        bIsInteraction = inputValue.isPressed;
        if(bIsInteraction)
        {
            OnInteractionEvent?.Invoke();
        }
    }

    public void OnInventory()
    {
        OnInventoryEvent?.Invoke();
    }
}
