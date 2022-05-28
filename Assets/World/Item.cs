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
    public UnityEvent OnPick;

    [SerializeField]
    bool isPickable = true;

    private SphereCollider sphereCollider;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    public Item TryPick()
    {
        if (!isPickable)
            return null;

        sphereCollider.enabled = false;

        OnPick.Invoke();

        return this;
    }

    public void SetIsPickable(bool value)
    {
        isPickable = true;
    }
}
