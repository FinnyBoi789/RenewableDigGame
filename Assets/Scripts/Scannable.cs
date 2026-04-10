using UnityEngine;

public class Scannable : MonoBehaviour
{
    [SerializeField] private ScannableData data;

    public string objectName => data.objectName;
    public string objectDescription => data.objectDescription;
}