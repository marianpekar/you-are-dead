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

    public void TryPlaceSelectedItem(Inventory inventory)
    {
        GameObject itemGo = inventory.GetSelectedItem();

        if (itemGo)
        {
            Item item = itemGo.GetComponent<Item>();

            if(expectedItemName.Equals(item.Name))
            {
                item.transform.position = gameObject.transform.position + PlacePositionOffset;
                item.transform.rotation = Quaternion.Euler(PlaceRotation);

                inventory.RemoveSelectedItem();

                OnItemPlaced.Invoke();
            }
        }
    }
}
