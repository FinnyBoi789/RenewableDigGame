using UnityEngine;
using TMPro;

public class ProgressScreen : MonoBehaviour
{
    [SerializeField] private GameObject textPrefab; 
    [SerializeField] private Transform content;      

    public void AddEntry(string text)
    {
        GameObject entry = Instantiate(textPrefab, content);
        entry.GetComponent<TextMeshProUGUI>().text = text;
    }
}