using UnityEngine;
using TMPro;

public class IndexScreen : MonoBehaviour
{
    [SerializeField] private GameObject textPrefab; 
    [SerializeField] private Transform content;      

    public void AddEntry(string text)
    {
        GameObject entry = Instantiate(textPrefab, content);
        entry.GetComponent<TextMeshProUGUI>().text = text;
    }
}