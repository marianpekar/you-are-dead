using UnityEngine;
using UnityEngine.Events;

public class Socket : MonoBehaviour
{
    public float InteractableDistance = 1.5f;

    [SerializeField]
    private string expectedItemName;

    [SerializeField]
    private Vector3 placePositionOffset;

    [SerializeField]
    private Vector3 placeRotation;

    public UnityEvent OnItemPlaced;

    public UnityEvent OnItemRemoved;

    private Item currentItem;

    private bool isLocked;

    public Item Interact(Inventory inventory)
    {
        if (isLocked)
            return null;

        isLocked = true;
        Invoke(nameof(Unlock), 0.5f);

        if (currentItem)
        {
            return TakeItem();
        }

        TryPlaceItem(inventory);

        return null;
    }

    private void TryPlaceItem(Inventory inventory)
    {
        GameObject itemGo = inventory.GetSelectedItem();

        if (itemGo)
        {
            Item item = itemGo.GetComponent<Item>();

            if (expectedItemName.Equals(item.Name))
            {
                currentItem = item;

                currentItem.transform.position = gameObject.transform.position + placePositionOffset;
                currentItem.transform.rotation = Quaternion.Euler(placeRotation);

                inventory.RemoveSelectedItem();

                OnItemPlaced.Invoke();
            }
        }
    }

    private Item TakeItem()
    {
        if (!currentItem)
            return null;
        
        Item returnItem = currentItem.TryPick();

        currentItem = null;

        OnItemRemoved.Invoke();

        return returnItem;
    }

    private void Unlock()
    {
        isLocked = false;
    }
}
