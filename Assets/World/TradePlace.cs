using UnityEngine;
using UnityEngine.Events;

public class TradePlace : MonoBehaviour
{
    public float InteractableDistance = 1.5f;

    [SerializeField]
    private string expectedItemName;

    [SerializeField]
    private Item itemToTrade;

    [SerializeField]
    private Vector3 placePositionOffset;

    [SerializeField]
    private Vector3 placeRotation;

    private bool IsTradeDone;

    public UnityEvent OnTradeDone;

    private void Start()
    {
        itemToTrade.SetIsPickable(false);
    }

    public void TryTrade(Inventory inventory)
    {
        if (IsTradeDone)
            return;

        GameObject itemFromInventory = inventory.GetSelectedItem();

        if (itemFromInventory)
        {
            Item item = itemFromInventory.GetComponent<Item>();

            if (expectedItemName.Equals(item.Name))
            {
                IsTradeDone = true;

                inventory.RemoveSelectedItem();
                inventory.AddItem(itemToTrade.gameObject);

                itemFromInventory.transform.parent = transform;
                itemFromInventory.transform.position = transform.position + placePositionOffset;
                itemFromInventory.transform.rotation = Quaternion.Euler(placeRotation);

                itemToTrade.SetIsPickable(true);

                OnTradeDone.Invoke();
            }
        }
    }
}
