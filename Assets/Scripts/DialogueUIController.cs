using UnityEngine;
using TMPro;

public class DialogueUIController : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    public void ShowLine(string text)
    {
        Debug.Log("panel: " + panel);
        Debug.Log("dialogueText: " + dialogueText);
        
        panel.SetActive(true);
        dialogueText.text = text;
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
