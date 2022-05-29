using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] inventorySlots;

    [SerializeField]
    private Hover[] inventoryCameraHovers;


    [SerializeField]
    private Image endGameOverlay;

    [SerializeField]
    private Text endGameTextField;

    private string endGameText;
    private int currentEndGameTextCharIndex = 0;

    [SerializeField]
    private float endGameTextTypeSpeed = 0.2f;


    [SerializeField]
    private float endGameTextTypeStartDelay = 2f;

    private float currentOverlayAlpha = 0f;

    [SerializeField]
    private float overlayFadeInSpeed = 0.001f;

    [SerializeField]
    private float overlayFadeInStepSize = 0.01f;


    UnityEvent OnEndGameCharTyped;

    private void Start()
    {
        endGameText = endGameTextField.text;
        endGameTextField.text = string.Empty;
    }

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

    public void ShowEndGameOverlay()
    {
        InvokeRepeating(nameof(ShowEndGameOverlayRoutine), overlayFadeInSpeed, overlayFadeInSpeed);
    }

    private void ShowEndGameOverlayRoutine()
    {
        currentOverlayAlpha += overlayFadeInStepSize;

        if (currentOverlayAlpha >= 1.0f)
        {
            CancelInvoke(nameof(ShowEndGameOverlayRoutine));
            return;
        }

        endGameOverlay.color = new Color(endGameOverlay.color.r, endGameOverlay.color.g, endGameOverlay.color.b, currentOverlayAlpha);
    }

    public void TypeEndGameText()
    {
        endGameTextField.enabled = true;
        InvokeRepeating(nameof(TypeEndGameTextRoutine), endGameTextTypeStartDelay, endGameTextTypeSpeed);
    }

    private void TypeEndGameTextRoutine()
    {
        endGameTextField.text = endGameTextField.text + endGameText[currentEndGameTextCharIndex];

        OnEndGameCharTyped?.Invoke();

        currentEndGameTextCharIndex++;

        if(currentEndGameTextCharIndex >= endGameText.Length)
        {
            CancelInvoke(nameof(TypeEndGameTextRoutine));
        }
    }

    public void DisableAllInventorySlots()
    {
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            DisableInventorySlot(i);
        }
    }
}
