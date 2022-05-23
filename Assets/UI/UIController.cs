using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] inventorySlots;

    [SerializeField]
    private Hover[] inventoryCameraHovers;

    public void EnableInventorySlot(int i)
    {
        inventorySlots[i].SetActive(true);
    }

    public void DisableInventorySlot(int i)
    {
        inventorySlots[i].SetActive(false);
        inventoryCameraHovers[i].enabled = false;
    }

    public void EnableInventoryCameraHover(int i)
    {
        inventoryCameraHovers[i].enabled = true;
    }

    public void DisableInventoryCameraHover(int i)
    {
        inventoryCameraHovers[i].enabled = false;
    }
}
