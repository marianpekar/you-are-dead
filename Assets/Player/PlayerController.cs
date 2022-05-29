using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Inventory))]
public class PlayerController : MonoBehaviour
{
    private Inventory inventory;
    private NavMeshAgent agent;

    private Item targetItem;
    private Socket targetSocket;
    private Rotatable targetRotatable;
    private Breakable targetBreakable;
    private TradePlace targetTradePlace;
    private EndGameTrigger targetEndGameTrigger;

    private Camera mainCamera;
    private SkinnedMeshRenderer meshRenderer;
    private PlayerAnimationController playerAnimationController;

    public UnityEvent OnTeleportEnd;
    public UnityEvent OnTeleportBegin;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        inventory = GetComponent<Inventory>();
        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        playerAnimationController = GetComponent<PlayerAnimationController>();

        mainCamera = Camera.main;

        OnTeleportBegin.AddListener(() => { agent.enabled = false; });
        OnTeleportEnd.AddListener(() => { agent.enabled = true; });
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        if (EventSystem.current.IsPointerOverGameObject() || !Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hit, 1000)) return;

        Debug.DrawLine(mainCamera.ScreenToWorldPoint(Input.mousePosition), hit.point, Color.red);

        Breakable breakable = hit.collider.gameObject.GetComponent<Breakable>();
        if(breakable)
        {
            if (Vector3.Distance(gameObject.transform.position, breakable.transform.position) <= breakable.InteractableDistance)
            {
                SetTarget(gameObject.transform.position);
                breakable.TryBreak(inventory);
                return;
            }
            else
            {
                targetBreakable = breakable;
            }
        }

        TeleportSpell teleportSpell = hit.collider.gameObject.GetComponent<TeleportSpell>();
        if(teleportSpell)
        {
            SetTarget(gameObject.transform.position);
            teleportSpell.Teleport(gameObject);
            return;
        }

        Rotatable rotatable = hit.collider.gameObject.GetComponent<Rotatable>();
        if(rotatable)
        {
            if(Vector3.Distance(gameObject.transform.position, rotatable.transform.position) <= rotatable.InteractableDistance)
            {
                SetTarget(gameObject.transform.position);
                rotatable.Rotate();
                return;
            }
            else
            {
                targetRotatable = rotatable;
            }
        }

        Socket socket = hit.collider.gameObject.GetComponent<Socket>();
        if (socket)
        {
            if (Vector3.Distance(gameObject.transform.position, socket.transform.position) <= socket.InteractableDistance)
            {
                SetTarget(gameObject.transform.position);
                Item itemFromSocket = socket.Interact(inventory);
                if(itemFromSocket)
                {
                    inventory.AddItem(itemFromSocket.gameObject);
                }

                return;
            }
            else
            {
                targetSocket = socket;
            }
        }

        TradePlace tradePlace = hit.collider.gameObject.GetComponent<TradePlace>();
        if(tradePlace)
        {
            if (Vector3.Distance(gameObject.transform.position, tradePlace.transform.position) <= tradePlace.InteractableDistance)
            {
                SetTarget(gameObject.transform.position);
                tradePlace.TryTrade(inventory);
                return;
            }
            else
            {
                targetTradePlace = tradePlace;
            }
        }

        Item item = hit.collider.gameObject.GetComponent<Item>();
        if (item)
        {
            targetItem = item;
        }

        EndGameTrigger endGameTrigger = hit.collider.gameObject.GetComponent<EndGameTrigger>();
        if(endGameTrigger)
        {
            if(Vector3.Distance(gameObject.transform.position, endGameTrigger.transform.position) <= endGameTrigger.InteractableDistance)
            {
                endGameTrigger.EndGame();
                return;
            }
            else
            {
                targetEndGameTrigger = endGameTrigger;
            }
        }

        SetTarget(hit.point);
    }

    void FixedUpdate()
    {
        if (targetBreakable && Vector3.Distance(gameObject.transform.position, targetBreakable.transform.position) <= targetBreakable.InteractableDistance)
        {
            SetTarget(gameObject.transform.position);

            targetBreakable.TryBreak(inventory);
            targetBreakable = null;
            return;
        }

        if (targetItem && Vector3.Distance(gameObject.transform.position, targetItem.transform.position) <= targetItem.InteractableDistance)
        {
            SetTarget(gameObject.transform.position);

            GameObject pickableGo = targetItem.TryPick().gameObject;
            targetItem = null;
            inventory.AddItem(pickableGo);
            return;
        }

        if (targetSocket && Vector3.Distance(gameObject.transform.position, targetSocket.transform.position) <= targetSocket.InteractableDistance)
        {
            SetTarget(gameObject.transform.position);

            Item itemFromSocket = targetSocket.Interact(inventory);
            if (itemFromSocket)
            {
                inventory.AddItem(itemFromSocket.gameObject);
            }

            targetSocket = null;
            return;
        }

        if (targetTradePlace && Vector3.Distance(gameObject.transform.position, targetTradePlace.transform.position) <= targetTradePlace.InteractableDistance)
        {
            SetTarget(gameObject.transform.position);

            targetTradePlace.TryTrade(inventory);

            targetTradePlace = null;
            return;
        }

        if (targetRotatable && Vector3.Distance(gameObject.transform.position, targetRotatable.transform.position) <= targetRotatable.InteractableDistance)
        {
            SetTarget(gameObject.transform.position);

            targetRotatable.Rotate();

            targetRotatable = null;
            return;
        }

        if (targetEndGameTrigger && Vector3.Distance(gameObject.transform.position, targetEndGameTrigger.transform.position) <= targetEndGameTrigger.InteractableDistance)
        {
            SetTarget(gameObject.transform.position);

            targetEndGameTrigger.EndGame();
            targetEndGameTrigger = null;
        }
    }

    private void SetTarget(Vector3 target)
    {
        agent.destination = target;
    }

    public void DisableAgent()
    {
        playerAnimationController.enabled = false;
        agent.enabled = false;
    }
    
    public void HideMesh()
    {
        meshRenderer.enabled = false;
    }
}
