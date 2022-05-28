using UnityEngine;
using UnityEngine.Events;

public class Breakable : MonoBehaviour
{
    public float InteractableDistance = 1.5f;

    [SerializeField]
    private string expectedToolToBrakeWith;

    public UnityEvent OnBreak;

    private bool isLocked = false;

    public void TryBreak(Inventory inventory)
    {
        if (isLocked)
            return;

        isLocked = true;
        Invoke(nameof(Unlock), 0.5f);

        Item selectedInventoryItem = inventory.GetSelectedItem().GetComponent<Item>(); 

        if(selectedInventoryItem.Name.Equals(expectedToolToBrakeWith))
        {
            OnBreak.Invoke();
            gameObject.SetActive(false);
        }
    }

    private void Unlock()
    {
        isLocked = false;
    }
}
