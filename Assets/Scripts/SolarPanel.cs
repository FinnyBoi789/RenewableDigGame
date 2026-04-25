using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SolarPanel : MonoBehaviour
{
    [SerializeField] Transform panelSurface; // the face of the panel
    [SerializeField] Vector3 sunDirection = new Vector3(1, 1, 0).normalized;

    [SerializeField] private Renderer renderer;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform shipTarget;

    [SerializeField] private AudioSource humAudioSource;
    [SerializeField] private AudioClip humSound;
    [SerializeField] private AudioClip connectedSound;
    bool isHumPlaying = false;

    [SerializeField] float alignmentThreshold = 0.8f;

    [SerializeField] XRGrabInteractable grabInteractable;
    [SerializeField] Rigidbody rb;

    float alignmentTimer = 0f;
    [SerializeField] float requiredAlignmentTime = 0.5f;

    bool isLocked = false;

    bool hasCounted = false;


    void Start()
    {
        if(lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }

    void Update()
    {
        if (!isLocked)
        {
            float alignment = Vector3.Dot(panelSurface.forward, sunDirection);

            if (alignment > alignmentThreshold)
            {
                alignmentTimer += Time.deltaTime;
                renderer.material.color = Color.yellow;

                if (alignmentTimer >= requiredAlignmentTime)
                {
                    LockPanel();
                }
            }
            else
            {
                alignmentTimer = 0f;
                renderer.material.color = Color.red;
            }
        }

        if (isLocked)
        {
            HandleBeam();
            HandleHum();
        }
    }

    void HandleBeam()
    {
        if (lineRenderer != null && shipTarget != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, panelSurface.position);
            lineRenderer.SetPosition(1, shipTarget.position);
        }
    }

    void HandleHum()
    {
        if (!isHumPlaying)
        {
            humAudioSource.clip = humSound;
            humAudioSource.loop = true;
            humAudioSource.Play();
            isHumPlaying = true;
        }
    }

    void ActivatePanel()
    {
        Debug.Log("Panel aligned");

        if (!hasCounted)
        {
            humAudioSource.PlayOneShot(connectedSound);
            hasCounted = true;
        }
    }

    void LockPanel()
    {
        isLocked = true;

        grabInteractable.enabled = false;
        rb.isKinematic = true;

        transform.rotation = Quaternion.LookRotation(sunDirection);

        renderer.material.color = Color.green;
        ActivatePanel();

        GameManager.Instance.PanelAligned();
    }
}
