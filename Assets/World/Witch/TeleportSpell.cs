using UnityEngine;

public class TeleportSpell : MonoBehaviour
{
    private bool canTeleport;

    [SerializeField]
    private Transform[] teleportTargets;

    private int currentTeleportTargetIndex = 0;

    [SerializeField]
    private string onTeleportAnimationTriggerName = "Interact";

    [SerializeField]
    private float delay = 1f;

    private bool isTeleporting;

    private GameObject objectToTeleport;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetCanTeleport(bool value)
    {
        canTeleport = value;
    }

    public void Teleport(GameObject objectToTeleport)
    {
        if (!canTeleport || isTeleporting)
            return;

        isTeleporting = true;

        animator.SetTrigger(onTeleportAnimationTriggerName);

        this.objectToTeleport = objectToTeleport;
        Invoke(nameof(TeleportRoutine), delay);
    }

    private void TeleportRoutine()
    {
        PlayerController playerController = objectToTeleport.GetComponent<PlayerController>();

        playerController?.OnTeleportBegin.Invoke();

        objectToTeleport.transform.position = teleportTargets[currentTeleportTargetIndex].position;
        objectToTeleport.transform.rotation = teleportTargets[currentTeleportTargetIndex].rotation;
        objectToTeleport = null;

        playerController?.OnTeleportEnd.Invoke();

        SetNextTeleportTarget();
        Invoke(nameof(UnsetIsTeleporting), 0.5f);
    }

    private void UnsetIsTeleporting()
    {
        isTeleporting = false;
    }

    private void SetNextTeleportTarget()
    {
        currentTeleportTargetIndex++;
        if (currentTeleportTargetIndex >= teleportTargets.Length)
            currentTeleportTargetIndex = 0;
    }
}
