using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class Scanner : MonoBehaviour
{
    [SerializeField] XRRayInteractor rayInteractor;
    [SerializeField] InputActionProperty triggerAction;
    [SerializeField] UIManager uiManager;
    [SerializeField] IndexScreen indexScreen;

    float loseTargetTimer = 0f;
    float loseTargetDelay = 0.2f;

    Scannable currentTarget;

    void Update()
    {
        if (triggerAction.action.IsPressed())
        {
            if (Scan())
            {
                loseTargetTimer = 0f;
            }
            else
            {
                loseTargetTimer += Time.deltaTime;

                if (loseTargetTimer >= loseTargetDelay)
                    ResetScan();
            }
        }
        else
        {
            ResetScan();
        }        
    }

    bool Scan()
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            Scannable scannable = hit.collider.GetComponentInParent<Scannable>();

            if (scannable != null)
            {
                if (currentTarget != scannable)
                {
                    ResetScan();
                    currentTarget = scannable;
                }

                if (!scannable.isScanned)
                {
                    scannable.currentScan += Time.deltaTime;

                    float percent = scannable.currentScan / scannable.scanDuration;
                    uiManager.ShowScanProgress(percent);

                    if (percent >= 1f)
                    {
                        scannable.isScanned = true;
                        uiManager.ShowInfo(scannable.objectName, scannable.objectDescription);
    
                        if (!scannable.isLogged)
                        {
                            indexScreen.AddEntry(scannable.objectName + ": " + scannable.objectDescription);
                            scannable.isLogged = true;
                            GameManager.Instance.LogScannedObject();

                            if (GameManager.Instance.CurrentState == GameState.HasScanner)
                            {
                                GameManager.Instance.SetState(GameState.FirstScanDone);
                            }
                        }
                    }
                }
                else
                {
                    uiManager.ShowInfo(scannable.objectName, scannable.objectDescription);
                }
                return true;
            }
           
        }

        ResetScan();
        return false;
    }

    void ResetScan()
    {
        currentTarget = null;
        uiManager.HideInfo();
        uiManager.HideProgress();
    }
}