using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Scanner : MonoBehaviour
{
    [SerializeField] XRRayInteractor rayInteractor;
    [SerializeField] InputActionProperty triggerAction;
    [SerializeField] UIManager uiManager;

    void Update()
    {
        if (triggerAction.action.WasPressedThisFrame())
            TryScan();
    }

    void TryScan()
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            Scannable scannable = hit.collider.GetComponent<Scannable>();
            if (scannable != null)
            {
                uiManager.ShowInfo(scannable.objectName, scannable.objectDescription);
            }
            else
            {
                uiManager.HideInfo(); // hit something, but it's not scannable
            }
        }
        else
        {
            uiManager.HideInfo();
        }
    }
}