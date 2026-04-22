using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using TMPro;

public class ObjectsScannedScript : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private TMPro.TextMeshProUGUI objectsScanned;

    void Update()
    {
        int scanned = GameManager.Instance.ObjectsScanned;
        int total = GameManager.Instance.TotalObjects;

        if (scanned >= total)
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
