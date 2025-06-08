using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image ItemIcon;
    public Text ItemQuantity;

    Button ItemButton;


    public void InitializeItem()
    {
        ItemButton = GetComponent<Button>();

        ItemIcon.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        ItemQuantity.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

    public void SetitemData(PickUpItem item)
    {
        if (item)
        {
            ItemIcon.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            ItemQuantity.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            ItemIcon.sprite = item.itemIcon;
            ItemQuantity.text = $"{item.itemQuantity}";
        }
    }

    public void OnClickedItemButton()
    {
        Debug.Log("Clicked");
    }
}
