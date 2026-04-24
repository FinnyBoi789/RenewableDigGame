using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using TMPro;
using System.Runtime.CompilerServices;

public class ObjectsScannedScript : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ScannerController scannerController;

    [SerializeField] private TMPro.TextMeshProUGUI objectsScanned;
    [SerializeField] private TMPro.TextMeshProUGUI scannerMode;


    void Update()
    {
        int scanned = GameManager.Instance.ObjectsScanned;
        int total = GameManager.Instance.TotalObjects;

        GameState state = GameManager.Instance.CurrentState;

        scannerMode.text = "Current Mode: " + scannerController.CurrentMode;

        if (state == GameState.waypointTriggered)
        {
            objectsScanned.text = "Environment Scanned!";
            return;
        }

        if (scanned >= total && state != GameState.ScannedEnvironment)
        {
            objectsScanned.text = "Environment Scanned!";
            GameManager.Instance.SetState(GameState.ScannedEnvironment);
        }
        else
        {
            objectsScanned.text = "Objects Scanned: " + scanned + "/" + total;
        }
    }
}
