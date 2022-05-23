using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Inventory))]
public class PlayerController : MonoBehaviour
{
    private Inventory inventory;
    private NavMeshAgent agent;

    private Item targetItem;
    private Socket targetSocket;

    private Camera mainCamera;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        inventory = GetComponent<Inventory>();

        mainCamera = Camera.main;
    }


    void Update()
    {
        if (!Input.GetMouseButton(0)) return;

        if (EventSystem.current.IsPointerOverGameObject() || !Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out var hit, 1000)) return;

        Debug.DrawLine(mainCamera.ScreenToWorldPoint(Input.mousePosition), hit.point, Color.red);

        Socket socket = hit.collider.gameObject.GetComponent<Socket>();
        if (socket)
        {
            if (Vector3.Distance(gameObject.transform.position, socket.transform.position) <= socket.InteractableDistance)
            {
                socket.TryPlaceSelectedItem(inventory);
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
        }

        if (targetSocket && Vector3.Distance(gameObject.transform.position, targetSocket.transform.position) <= targetSocket.InteractableDistance)
        {
            agent.destination = gameObject.transform.position;

            targetSocket.TryPlaceSelectedItem(inventory);
            targetSocket = null;
            return;
        }
    }

    private void SetTarget(Vector3 target)
    {
        agent.destination = target;
    } 
}
