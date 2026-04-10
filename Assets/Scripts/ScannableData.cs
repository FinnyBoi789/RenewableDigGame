using UnityEngine;

[CreateAssetMenu(fileName = "NewScannableData", menuName = "Scanner/Scannable Data")]
public class ScannableData : ScriptableObject
{
    public string objectName;
    [TextArea] public string objectDescription;
}