using UnityEngine;
using UnityEngine.Events;

public class Socket : MonoBehaviour
{
    public float InteractableDistance = 1.5f;

    [SerializeField]
    private string expectedItemName;

    [SerializeField]
    private Vector3 PlacePositionOffset;

    [SerializeField]
    private Vector3 PlaceRotation;

    public UnityEvent OnItemPlaced;

    public UnityEvent OnItemRemoved;

    private Item item;

    private bool isLocked;

    public Item Interact(Inventory inventory)
    {
        if (isLocked)
            return null;

        isLocked = true;
        Invoke(nameof(Unlock), 0.5f);

        if (item)
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
            item = itemGo.GetComponent<Item>();

            if (expectedItemName.Equals(item.Name))
            {
                item.transform.position = gameObject.transform.position + PlacePositionOffset;
                item.transform.rotation = Quaternion.Euler(PlaceRotation);

                inventory.RemoveSelectedItem();

                OnItemPlaced.Invoke();
            }
        }
    }

    private Item TakeItem()
    {
        if (!item)
            return null;
        
        Item returnItem = item.Pick();

        item = null;

        OnItemRemoved.Invoke();

        return returnItem;
    }

    private void Unlock()
    {
        isLocked = false;
    }
}
