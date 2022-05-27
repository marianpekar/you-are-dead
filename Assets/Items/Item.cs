using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class Item : MonoBehaviour
{
    public string Name;

    public float InteractableDistance = 1.5f;

    public Vector3 InventoryRotation;

    public UnityEvent OnPlace;

    private SphereCollider sphereCollider;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    public Item Pick()
    {
        sphereCollider.enabled = false;

        return this;
    }
}
