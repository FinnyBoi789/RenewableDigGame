using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FloatAndBob : MonoBehaviour
{
    [SerializeField] private float bobHeight = 0.1f;
    [SerializeField] private float bobSpeed = 1f;
    [SerializeField] private float rotateSpeed = 45f;

    private Vector3 startPosition;
    private XRGrabInteractable grabInteractable;
    
    bool hasInitialised = false;
    // Update is called once per frame
    void Update()
    {
        if (!hasInitialised)
        {
            startPosition = transform.position;
            hasInitialised = true;
            return;
        }

        if(grabInteractable != null && grabInteractable.isSelected)
            return;

        transform.position = startPosition + Vector3.up * Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}
