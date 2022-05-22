using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(MeshRenderer))]
public class Picklable : MonoBehaviour
{
    public string Title;
    public float PickableDistance = 1.5f;

    public Vector3 InventoryRotation;

    public UnityEvent OnPick;

    private SphereCollider sphereCollider;

    private void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    public Picklable Pick()
    {
        sphereCollider.enabled = false;

        OnPick.Invoke();

        return this;
    }
}
