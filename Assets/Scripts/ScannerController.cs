using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class ScannerController : MonoBehaviour
{
    public XRRayInteractor rayInteractor;

    XRGrabInteractable grabInteractable;

    [SerializeField] UIManager uiManager;

    [SerializeField] private DialogueSequenceData scannerDialogue;
    [SerializeField] private DialogueSequenceData firstScanDialogue;
    bool isHeld;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        Debug.Log("grabInteractable: " + grabInteractable, gameObject);

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        grabInteractable.activated.AddListener(OnActivated);
        grabInteractable.deactivated.AddListener(OnDeactivated);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true;
        if (GameManager.Instance.CurrentState == GameState.Crashed)
        {
            GameManager.Instance.SetState(GameState.HasScanner);
            DialogueManager.Instance.PlaySequence(scannerDialogue);
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;
        rayInteractor.enabled = false;
        uiManager.HideInfo();
    }

    void OnActivated(ActivateEventArgs args)
    {
        if (isHeld)
        {
            rayInteractor.enabled = true;
        }    
    }

    void OnDeactivated(DeactivateEventArgs args)
    {
        rayInteractor.enabled = false;
        uiManager.HideInfo();
        if (GameManager.Instance.CurrentState == GameState.HasScanner)
        {
            GameManager.Instance.SetState(GameState.FirstScanDone);
            if (GameManager.Instance.CurrentState == GameState.FirstScanDone)
            {
                DialogueManager.Instance.PlaySequence(firstScanDialogue);
                Debug.Log("Current State: " + GameManager.Instance.CurrentState);
            } else
            {
                return;
            }
        }
    }
}