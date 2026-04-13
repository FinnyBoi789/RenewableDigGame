using UnityEngine;

public class Scannable : MonoBehaviour
{
    [SerializeField] private ScannableData data;

    public string objectName => data.objectName;
    public string objectDescription => data.objectDescription;

    public float scanDuration = 2f;

    [HideInInspector] public float currentScan = 0f;
    [HideInInspector] public bool isScanned = false;
    [HideInInspector] public bool isLogged = false;
}