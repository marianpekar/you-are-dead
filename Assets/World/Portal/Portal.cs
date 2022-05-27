using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    private GameObject fill;

    private MeshRenderer meshRenderer;


    [SerializeField]
    private float dissolveSpeed = 0.01f;

    [SerializeField]
    private float dissolveStepSize = 0.01f;

    private float currentDissolve;

    private float startingDissolve;


    [SerializeField]
    private Light[] lights;

    [SerializeField]
    private float dimLightsSpeed = 0.01f;

    [SerializeField]
    private float dimLightsStepSize = 0.01f;

    private float[] startingLightIntensities;

    private bool isOff;

    void Start()
    {
        meshRenderer = fill.GetComponent<MeshRenderer>();
        startingDissolve = currentDissolve = meshRenderer.material.GetFloat("_Amount");

        List<float> currentLightIntensities = new List<float>();
        foreach (var light in lights)
        {
            currentLightIntensities.Add(light.intensity);
        }
        startingLightIntensities = currentLightIntensities.ToArray();
    }

    public void TurnOff()
    {
        isOff = true;
    }

    public void TurnOn()
    {
        isOff = false;
    }

    public void Open()
    {
        if (isOff)
            return;

        InvokeRepeating(nameof(DissolveRepeating), dissolveSpeed, dissolveSpeed);
        InvokeRepeating(nameof(DimLightsRepeating), dimLightsSpeed, dimLightsSpeed);
    }

    public void Close()
    {
        if (isOff)
            return;

        InvokeRepeating(nameof(AppearRepeating), dissolveSpeed, dissolveSpeed);
        InvokeRepeating(nameof(BrightenLightsRepeating), dimLightsSpeed, dimLightsSpeed);
    }

    private void DissolveRepeating()
    {
        currentDissolve += dissolveStepSize;
        meshRenderer.material.SetFloat("_Amount", currentDissolve);

        if (currentDissolve >= 1.0f)
        {
            CancelInvoke(nameof(DissolveRepeating));
            fill.SetActive(false);
        }
    }

    private void AppearRepeating()
    {
        fill.SetActive(true);

        currentDissolve -= dissolveStepSize;
        meshRenderer.material.SetFloat("_Amount", currentDissolve);

        if (currentDissolve <= startingDissolve)
        {
            CancelInvoke(nameof(AppearRepeating));
        }
    }

    private void DimLightsRepeating()
    {
        foreach (var light in lights)
        {
            light.intensity -= dimLightsStepSize;
        }

        bool cancelInvoke = true;
        foreach (var light in lights)
        {
            if (light.intensity > 0f)
                cancelInvoke = false;
        }

        if(cancelInvoke)
        {
            CancelInvoke(nameof(DimLightsRepeating));
        }
    }

    private void BrightenLightsRepeating()
    {
        foreach (var light in lights)
        {
            light.intensity += dimLightsStepSize;
        }

        bool cancelInvoke = true;
        for (int i = 0; i < lights.Length; i++)
        {
            if (lights[i].intensity < startingLightIntensities[i])
                cancelInvoke = false;
        }

        if (cancelInvoke)
        {
            CancelInvoke(nameof(BrightenLightsRepeating));
        }
    }
}
