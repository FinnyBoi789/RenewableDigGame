using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using System.Collections;
using UnityEngine.UI;
using System.ComponentModel;
enum ScanResult
{
    None,
    Viewing,
    Scanning
}
public class Scanner : MonoBehaviour
{
    [SerializeField] XRRayInteractor rayInteractor;
    [SerializeField] InputActionProperty triggerAction;
    [SerializeField] UIManager uiManager;
    [SerializeField] IndexScreen indexScreen;
    [SerializeField] AudioSource scannerAudioSource;
    [SerializeField] AudioClip scanningAudioClip;
    [SerializeField] ScannerController scannerController;
    [SerializeField] GameObject notMineable;

    [SerializeField] private ParticleSystem miningParticles;

    float loseTargetTimer = 0f;
    float loseTargetDelay = 0.2f;
    bool isScanningAudioPlaying = false;

    Scannable currentTarget;
    Mineable currentMineTarget;

    void Update()
        {
            if (triggerAction.action.IsPressed())
            {
                if (scannerController.CurrentMode == ScannerMode.Scan)
                {
                    ScanResult result = Scan();

                    if (result == ScanResult.Scanning)
                    {
                        loseTargetTimer = 0f;

                        if (!isScanningAudioPlaying)
                        {
                            scannerAudioSource.clip = scanningAudioClip;
                            scannerAudioSource.loop = true;
                            scannerAudioSource.Play();
                            isScanningAudioPlaying = true;
                        }
                    }
                    else if (result == ScanResult.Viewing)
                    {
                        loseTargetTimer = 0f;
                        StopScanAudio();
                    }
                    else
                    {
                        loseTargetTimer += Time.deltaTime;
                        if (loseTargetTimer >= loseTargetDelay)
                        {
                            StopScanAudio();
                            ResetScan();
                        }
                    }
                }
                else
                {
                    if (Mine()) loseTargetTimer = 0f;
                    else
                    {
                        loseTargetTimer += Time.deltaTime;
                        if (loseTargetTimer >= loseTargetDelay)
                            ResetMine();
                    }
                }
            }
            else
            {
                StopScanAudio();
                ResetScan();
                ResetMine();
            }
        }
    

    ScanResult Scan()
    {
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if (hit.collider == null) return ScanResult.None;
            Scannable scannable = hit.collider.GetComponentInParent<Scannable>();
            Debug.Log("Ray hit: " + hit.collider.name);

            if (scannable != null)
            {
                Debug.Log("isScanned: " + scannable.isScanned);
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
                        StopScanAudio();
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
                    return ScanResult.Scanning;
                }
                else
                {
                    uiManager.ShowInfo(scannable.objectName, scannable.objectDescription);
                    Debug.Log("No raycast hit");
                }
                return ScanResult.Viewing;
            }
           
        }

        ResetScan();
        return ScanResult.None;
    }

    void ResetScan()
    {
        currentTarget = null;
        uiManager.HideInfo();
        uiManager.HideProgress();
    }

    void StopScanAudio(){
        if (isScanningAudioPlaying)
        {
            scannerAudioSource.Stop();
            isScanningAudioPlaying = false;
        }
    }

    bool Mine()
    {
        if(rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
        {
            if (hit.collider == null) return false;
            Debug.Log("Mine ray hit: " + hit.collider.name);

            Mineable mineable = hit.collider.GetComponentInParent<Mineable>();
            Debug.Log("Mineable found: " + mineable);

            if(mineable != null && !mineable.isMined)
            {
                notMineable.SetActive(false);

                if (mineable.requiresTurbine && !scannerController.hasTurbine)
                {   
                    notMineable.SetActive(true);
                    return false;
                }

                miningParticles.transform.position = hit.point;

                if (!miningParticles.isPlaying)
                {
                    miningParticles.Play();
                }

                if (currentMineTarget != mineable)
                {
                    ResetMine();
                    currentMineTarget = mineable;
                }

                mineable.currentMine += Time.deltaTime;
                float percent = mineable.currentMine / mineable.mineDuration;
                uiManager.ShowMineProgress(percent);

                if (percent >= 1f)
                {
                    mineable.isMined = true;
                    mineable.SpawnDrop(hit.point);
                    miningParticles.Stop();
                    miningParticles.Clear();
                    StartCoroutine(ShrinkAndDestroy(mineable.gameObject));
                    ResetMine();
                }
                return true;
            }
            else
            {
                Debug.Log("Mine: no raycast hit");
            }
        }
        return false;
    }

    void ResetMine()
    {
        currentMineTarget = null;
        miningParticles.Stop();
        miningParticles.Clear();
        uiManager.HideProgress();
    }

    IEnumerator ShrinkAndDestroy(GameObject obj)
    {
        float duration = 1f;
        float elapsed = 0f;
        Vector3 originalScale = obj.transform.localScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            obj.transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, elapsed / duration);
            yield return null;
        }
        Destroy(obj);
    }
}