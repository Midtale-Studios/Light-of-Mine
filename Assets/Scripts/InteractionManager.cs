using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f;
    public LayerMask interactableLayer;

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Checking for interaction...");
            Debug.DrawRay(transform.position, transform.forward * interactionRange, Color.red);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, interactableLayer))
            {
                Debug.Log("Interacting with " + hit.collider.gameObject.name);
                
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    interactable.Interact();
                }
            }
        }
    }
}