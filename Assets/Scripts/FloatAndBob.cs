using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FloatAndBob : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float bobHeight = 0.1f;
    [SerializeField] private float bobSpeed = 1f;
    [SerializeField] private float rotateSpeed = 45f;

    private Vector3 startPosition;
    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;
    private bool hasInitialised = false;
    private bool isGrabbed = false;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;

        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    void OnGrabbed(UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs args)
    {
        isGrabbed = true;
        GameManager.Instance.SetState(GameState.pickedUpTurbine);
    }

    void OnReleased(UnityEngine.XR.Interaction.Toolkit.SelectExitEventArgs args)
    {
        isGrabbed = false;
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (!hasInitialised)
        {
            startPosition = transform.position;
            hasInitialised = true;
            return;
        }

        if (isGrabbed) return; // XR handles everything when grabbed

        // only zero velocity when not grabbed
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = false;

        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        rb.MovePosition(new Vector3(startPosition.x, newY, startPosition.z));
        rb.MoveRotation(rb.rotation * Quaternion.Euler(Vector3.up * rotateSpeed * Time.deltaTime));
    }

    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }
}