using UnityEngine;
using UnityEngine.Events;

public class EndGameTrigger : MonoBehaviour
{
    public float InteractableDistance = 2.0f;

    public UnityEvent OnEndGameTriggered;
    public UnityEvent OnEndGameTriggeredDelayed;

    [SerializeField]
    private float delay;

    public void EndGame()
    {
        OnEndGameTriggered.Invoke();

        Invoke(nameof(EndGameDelayed), delay);
    }

    private void EndGameDelayed()
    {
        OnEndGameTriggeredDelayed.Invoke();
    }
}
