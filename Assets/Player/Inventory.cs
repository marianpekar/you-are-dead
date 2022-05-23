using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    GameObject[] slots = new GameObject[10];

    public UnityEvent<int> OnItemAdded; 
    public UnityEvent<int> OnItemRemoved;
    public UnityEvent<int> OnItemSelected;
    public UnityEvent<int> OnItemDeselected;

    private int selectedItemIndex = -1;
    public bool HasSelectedItem { get => selectedItemIndex >= 0; }

    public void AddItem(GameObject itemGo) 
    {
        for (int i = 0; i < slots.Length; i++) 
        {
            if (slots[i] == null)
            {
                slots[i] = itemGo;

                Item item = itemGo.GetComponent<Item>();

                itemGo.transform.position = new Vector3(i, 100, 1);
                itemGo.transform.rotation = Quaternion.Euler(item.InventoryRotation);

                OnItemAdded.Invoke(i);
                
                return;
            }
        }
    }

    public GameObject GetSelectedItem()
    {
        if(HasSelectedItem)
        {
            return slots[selectedItemIndex];
        }

        return null;
    }

    public void RemoveSelectedItem() 
    {
        slots[selectedItemIndex] = null;

        OnItemRemoved.Invoke(selectedItemIndex);
    }

    public void SetSelectedItemIndex(int index)
    {
        if(HasSelectedItem)
        {
            OnItemDeselected.Invoke(selectedItemIndex);
        }

        selectedItemIndex = index;

        OnItemSelected.Invoke(index);
    }
}