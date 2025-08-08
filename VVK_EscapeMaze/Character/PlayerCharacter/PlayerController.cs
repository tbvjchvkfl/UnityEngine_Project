using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Vector2 inputDirection {  get; private set; }
    public Vector2 lookDirection { get; private set; }

    public bool bIsStep { get; set; } = false;
    public bool bIsDodge {  get; set; } = false;
    public bool bIsSprint {  get; set; } = false;
    public bool bIsAim { get; set; } = false;
    public bool bIsNormalAttack {  get; set; } = false;
    public bool bIsInteraction {  get; set; } = false;
    public bool bIsSkillReady { get; set; } = false;

    // Non-Control
    public bool bIsHit { get; set; } = false;
    public bool bIsDead { get; set; } = false;
    public bool bIsEquip { get; set; } = false;
    public bool bIsInventory { get; set; } = false;

    // Delegates and Events
    public delegate void OnNormalAttackDelegate();
    public event OnNormalAttackDelegate OnNormalAttackEvent;

    public delegate void OnSpecialSkillDelegate();
    public event OnSpecialSkillDelegate OnSpecialSkillEvent;

    public delegate void OnDodgeDelegate();
    public event OnDodgeDelegate OnDodgeEvent;

    public delegate void OnInteractionDelegate();
    public event OnInteractionDelegate OnInteractionEvent;

    public delegate void OnInventoryDelegate();
    public event OnInventoryDelegate OnInventoryEvent;

    public delegate void OnSkillReadyDelegate();
    public event OnSkillReadyDelegate OnSkillReadyEvent;

    void Awake()
    {
        bIsEquip = true;
    }

    void Update()
    {

    }

    public void OnMove(InputValue inputValue)
    {
        if (!bIsHit && !bIsDead && !bIsInventory)
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
        if (!bIsHit && !bIsDead && !bIsInventory)
        {
            bIsSprint = inputValue.isPressed;
        }
    }

    public void OnDodge(InputValue inputValue)
    {
        if (!bIsHit && !bIsDead && !bIsInventory)
        {
            if (bIsStep)
            {
                bIsDodge = true;
            }
            else
            {
                bIsStep = true;
            }
            OnDodgeEvent?.Invoke();
        }
    }

    public void OnSkillReady(InputValue inputValue)
    {
        if (!bIsHit && !bIsDead && !bIsInventory)
        {
            bIsSkillReady = inputValue.isPressed;
            bIsAim = false;
            OnSkillReadyEvent?.Invoke();
        }
    }

    public void OnAim(InputValue inputValue)
    {
        if (bIsHit || bIsDead || bIsInventory)
        {
            bIsAim = false;
        }
        else if (bIsSkillReady)
        {
            OnSpecialSkillEvent?.Invoke();
        }
        else
        {
            bIsAim = inputValue.isPressed;
        }
        
    }

    public void OnAttack(InputValue inputValue)
    {
        if (!bIsHit && !bIsDead && !bIsInventory)
        {
            bIsNormalAttack = inputValue.isPressed;
            if (bIsNormalAttack)
            {
                OnNormalAttackEvent?.Invoke();
            }
        }
    }

    public void OnInteraction(InputValue inputValue)
    {
        if(!bIsHit && !bIsDead)
        {
            bIsInteraction = inputValue.isPressed;
            if (bIsInteraction)
            {
                OnInteractionEvent?.Invoke();
            }
        }
    }

    public void OnInventory()
    {
        if (bIsInventory)
        {
            bIsInventory = false;
        }
        else
        {
            bIsInventory = true;
        }
        OnInventoryEvent?.Invoke();
    }
}
