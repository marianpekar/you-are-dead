using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Rotatable : MonoBehaviour
{
    public float InteractableDistance = 3f;

    [SerializeField]
    private Vector3 targetRotationEuler;

    private Quaternion targetRoation;
    private Quaternion startRotation;

    private float progress;

    [SerializeField]
    private float rotationSpeed = 0.001f;

    [SerializeField]
    private float rotationStep = 1f;


    public UnityEvent OnTargetRotationReached;
    public UnityEvent OnStartRotationReached;

    private bool isRotating;
    private bool isInTargetRotation;

    void Start()
    {
        targetRoation = Quaternion.Euler(targetRotationEuler);
        startRotation = transform.rotation;
    }

    public void Rotate()
    {
        if (isRotating)
            return;

        isRotating = true;

        InvokeRepeating(nameof(RotateRepeating), rotationSpeed, rotationSpeed);
    }

    public void RotateRepeating()
    {
        progress += rotationStep;

        if (!isInTargetRotation)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRoation, progress);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(targetRoation, startRotation, progress);
        }

        if(progress >= 1f)
        {
            progress = 0f;

            isInTargetRotation = !isInTargetRotation;
            if (isInTargetRotation)
            {
                OnTargetRotationReached.Invoke();
            }
            else
            {
                OnStartRotationReached.Invoke();
            }

            CancelInvoke(nameof(RotateRepeating));

            isRotating = false;
        }

    }
}
