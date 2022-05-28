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

    private Camera mainCamera;

    public UnityEvent OnTeleportEnd;
    public UnityEvent OnTeleportBegin;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        inventory = GetComponent<Inventory>();

        mainCamera = Camera.main;

        OnTeleportBegin.AddListener(() => { agent.enabled = false; });
        OnTeleportEnd.AddListener(() => { agent.enabled = true; });
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        if (EventSystem.current.IsPointerOverGameObject() || !Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hit, 1000)) return;

        Debug.DrawLine(mainCamera.ScreenToWorldPoint(Input.mousePosition), hit.point, Color.red);

        TeleportSpell teleportSpell = hit.collider.gameObject.GetComponent<TeleportSpell>();
        if(teleportSpell)
        {
            agent.destination = gameObject.transform.position;
            teleportSpell.Teleport(gameObject);
            return;
        }

        Rotatable rotatable = hit.collider.gameObject.GetComponent<Rotatable>();
        if(rotatable)
        {
            if(Vector3.Distance(gameObject.transform.position, rotatable.transform.position) <= rotatable.InteractableDistance)
            {
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

        Item item = hit.collider.gameObject.GetComponent<Item>();
        if (item)
        {
            targetItem = item;
        }

        SetTarget(hit.point);
    }

    void FixedUpdate()
    {
        if (targetItem && Vector3.Distance(gameObject.transform.position, targetItem.transform.position) <= targetItem.InteractableDistance)
        {
            agent.destination = gameObject.transform.position;

            GameObject pickableGo = targetItem.Pick().gameObject;
            targetItem = null;
            inventory.AddItem(pickableGo);
            return;
        }

        if (targetSocket && Vector3.Distance(gameObject.transform.position, targetSocket.transform.position) <= targetSocket.InteractableDistance)
        {
            agent.destination = gameObject.transform.position;

            Item itemFromSocket = targetSocket.Interact(inventory);
            if (itemFromSocket)
            {
                inventory.AddItem(itemFromSocket.gameObject);
            }

            targetSocket = null;
            return;
        }

        if (targetRotatable && Vector3.Distance(gameObject.transform.position, targetRotatable.transform.position) <= targetRotatable.InteractableDistance)
        {
            agent.destination = gameObject.transform.position;

            targetRotatable.Rotate();

            targetRotatable = null;
            return;
        }
    }

    private void SetTarget(Vector3 target)
    {
        agent.destination = target;
    }
}
