using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class OpenDoor : MonoBehaviour, Interactable
{
    public float rotationDuration = 1f; // Duration of the rotation in seconds
    private bool isOpen = false; // Track the door's state
    [SerializeField] private GameObject pivotPoint;

    public void Interact()
    {
        Debug.Log("Door has been interacted with!");
        if (isOpen)
        {
            StartCoroutine(RotateAroundPivot(pivotPoint.transform, new Vector3(0, 90, 0)));
        }
        else
        {
            StartCoroutine(RotateAroundPivot(pivotPoint.transform, new Vector3(0, -90, 0)));
        }
        isOpen = !isOpen; // Toggle the door's state
    }

    private IEnumerator RotateAroundPivot(Transform pivot, Vector3 rotation)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(rotation);
        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            float t = elapsedTime / rotationDuration;
            transform.RotateAround(pivot.position, Vector3.up, rotation.y * Time.deltaTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
    }
}