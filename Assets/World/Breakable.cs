using UnityEngine;
using UnityEngine.Events;

public class Breakable : MonoBehaviour
{
    public float InteractableDistance = 1.5f;

    [SerializeField]
    private string expectedToolToBrakeWith;

    public UnityEvent OnBreak;

    public void TryBreak(Inventory inventory)
    {
        Item selectedInventoryItem = inventory.GetSelectedItem().GetComponent<Item>(); 

        if(selectedInventoryItem.Name == expectedToolToBrakeWith)
        {
            OnBreak.Invoke();
            gameObject.SetActive(false);
        }
    }
}
