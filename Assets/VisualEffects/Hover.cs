using UnityEngine;

public class Hover : MonoBehaviour
{
    [SerializeField]
    private float magnitude;

    [SerializeField]
    private float speed;

    private Vector3 initialPosition;

    private void OnEnable()
    {
        initialPosition = transform.position;
    }

    private void OnDisable()
    {
        transform.position = initialPosition;
    }


    void Update()
    {
        transform.position = initialPosition + Vector3.up * magnitude * Mathf.Cos(Time.time * speed);
    }
}