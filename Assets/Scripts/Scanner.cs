
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Scanner : MonoBehaviour
{
    [SerializeField] XRRayInteractor rayInteractor;
    [SerializeField] InputActionProperty triggerAction;
    [SerializeField] UIManager uiManager;


    // Update is called once per frame
    void Update()
    {
       if(triggerAction.action.WasPressedThisFrame())
        {
            TryScan();
        }
    }
    
    bool IsTriggerPressed()
    {
        return rayInteractor.xrController.activateInteractionState.activatedThisFrame;
    }

    void TryScan()
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            Debug.Log("Hit: " + hit.collider.name);

            Scannable scannable = hit.collider.GetComponent<Scannable>();
            if (scannable != null)
            {
                Debug.Log("Scanned: " + Scannable.objectName);
                uiManager.ShowInfo(Scannable.objectName, Scannable.objectDescription);
            }
        }
        else
        {
            Debug.Log("No hit");
        }
    }
}
