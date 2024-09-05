using UnityEngine;
using System.Collections;

public class SimpleTorchController : MonoBehaviour
{
    [SerializeField] private Transform smallFire1;
    [SerializeField] private Transform smallFire2;
    [SerializeField] private Transform smallFire3;
    [SerializeField] private Transform mainFire;
    [SerializeField] private Light torchLight;
    [SerializeField] private BoxCollider triggerArea;

    [SerializeField] private float minScale = 0.1f;
    [SerializeField] private float maxScale = 1f;
    [SerializeField] private float minLightIntensity = 0.1f;
    [SerializeField] private float maxLightIntensity = 1f;
    [SerializeField] private float transitionDuration = 3f;

    private Vector3 smallFire1StartScale;
    private Vector3 smallFire2StartScale;
    private Vector3 smallFire3StartScale;
    private Vector3 mainFireStartScale;

    private void Start()
    {
        smallFire1StartScale = smallFire1.localScale;
        smallFire2StartScale = smallFire2.localScale;
        smallFire3StartScale = smallFire3.localScale;
        mainFireStartScale = mainFire.localScale;

        // Start with minimum scale and intensity
        SetTorchState(0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TransitionTorch(true));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(TransitionTorch(false));
        }
    }

    private IEnumerator TransitionTorch(bool increasing)
    {
        float elapsedTime = 0f;
        float startValue = increasing ? 0f : 1f;
        float endValue = increasing ? 1f : 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);
            float currentValue = Mathf.Lerp(startValue, endValue, t);

            SetTorchState(currentValue);

            yield return null;
        }

        SetTorchState(endValue);
    }

    private void SetTorchState(float t)
    {
        float scale = Mathf.Lerp(minScale, maxScale, t);
        float intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, t);

        smallFire1.localScale = smallFire1StartScale * scale;
        smallFire2.localScale = smallFire2StartScale * scale;
        smallFire3.localScale = smallFire3StartScale * scale;
        mainFire.localScale = mainFireStartScale * scale;

        torchLight.intensity = intensity;
    }
}