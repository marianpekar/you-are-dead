using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    GameObject[] slots = new GameObject[10];

    public UnityEvent<GameObject> OnItemAdded; 
    public UnityEvent<GameObject> OnItemRemoved;
    public UnityEvent<GameObject> OnTryToAddToFullInventory;

    public void AddItem(GameObject item) 
    {
        for (int i = 0; i < slots.Length; i++) 
        {
            if (slots[i] == null)
            {
                slots[i] = item;

                Picklable picklable = item.GetComponent<Picklable>();

                item.transform.position = new Vector3(i, 100, 1);
                item.transform.rotation = Quaternion.Euler(picklable.InventoryRotation);

                OnItemAdded.Invoke(item);
                
                return;
            }
        }

        OnTryToAddToFullInventory.Invoke(item);
    }

    public GameObject GetItem(int index)
    {
        GameObject item = slots[index];
        slots[index] = null;

        OnItemRemoved.Invoke(item);

        return item;
    }
}