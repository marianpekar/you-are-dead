using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Socket : MonoBehaviour
{
    public string ExpectedItemName;

    public float InteractableDistance = 1.5f;

    public Vector3 PlacePositionOffset;

    public void TryPlaceSelectedItem(Inventory inventory)
    {
        GameObject itemGo = inventory.GetSelectedItem();

        if (itemGo)
        {
            Item item = itemGo.GetComponent<Item>();

            if(ExpectedItemName.Equals(item.Name))
            {
                item.transform.position = gameObject.transform.position + PlacePositionOffset;
                inventory.RemoveSelectedItem();
            }
        }
    }
}
