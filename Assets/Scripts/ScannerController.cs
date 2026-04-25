using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.InputSystem;

public enum ScannerMode
    {
        Scan,
        Mine
    }

public class ScannerController : MonoBehaviour{


    public ScannerMode CurrentMode { get; private set; } = ScannerMode.Scan;
    public XRRayInteractor rayInteractor;

    [SerializeField] private InputActionProperty modeToggleAction;

    XRGrabInteractable grabInteractable;

    [SerializeField] UIManager uiManager;

    [SerializeField] private DialogueSequenceData scannerDialogue;
    [SerializeField] private DialogueSequenceData firstScanDialogue;
    bool isHeld;
    public bool hasTurbine = false;

    void Awake()
    {
        modeToggleAction.action.Enable();
        
        grabInteractable = GetComponent<XRGrabInteractable>();
        Debug.Log("grabInteractable: " + grabInteractable, gameObject);

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        grabInteractable.activated.AddListener(OnActivated);
        grabInteractable.deactivated.AddListener(OnDeactivated);

        modeToggleAction.action.performed += OnModeToggle;
    }

    void OnModeToggle(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Debug.Log("Mode toggle fired, isHeld: " + isHeld);
        if (!isHeld) return;

        if (CurrentMode == ScannerMode.Scan)
        {
            CurrentMode = ScannerMode.Mine;

        }
        else
        {
            CurrentMode = ScannerMode.Scan;
        }
        Debug.Log("Current Scanner Mode: " + CurrentMode);
    }

    void OnDestroy()
    {
        modeToggleAction.action.Disable();
        modeToggleAction.action.performed -= OnModeToggle;
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if(args.interactorObject is XRSocketInteractor)
        {
            return; // Ignore socket interactions
        }   

        isHeld = true;
        if (GameManager.Instance.CurrentState == GameState.Crashed)
        {
            GameManager.Instance.SetState(GameState.HasScanner);
            DialogueManager.Instance.PlaySequence(scannerDialogue);
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        if(args.interactorObject is XRSocketInteractor)
        {
            return; // Ignore socket interactions
        }
        
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