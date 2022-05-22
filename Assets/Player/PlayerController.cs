using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Inventory))]
public class PlayerController : MonoBehaviour
{
    private Inventory inventory;
    private NavMeshAgent agent;

    private Picklable selectedPickable;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        inventory = GetComponent<Inventory>();
    }


    void Update()
    {
        if (!Input.GetMouseButton(0)) return;

        if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 1000)) return;

        Picklable picklable = hit.collider.gameObject.GetComponent<Picklable>();
        if (picklable)
        {
            selectedPickable = picklable;
        }

        SetTarget(hit.point);
    }

    void FixedUpdate()
    {
        if (selectedPickable && Vector3.Distance(gameObject.transform.position, selectedPickable.transform.position) <= selectedPickable.PickableDistance)
        {
            agent.destination = gameObject.transform.position;

            GameObject pickableGo = selectedPickable.Pick().gameObject;
            selectedPickable = null;
            inventory.AddItem(pickableGo);
        }
    }

    private void SetTarget(Vector3 target)
    {
        agent.destination = target;
    } 
}
