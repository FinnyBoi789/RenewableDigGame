using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private DialogueUIController dialogueUIController;
    
    private bool isPlaying = false;

    void Awake()
    {
        Instance = this;
        Debug.Log("DialogueManager awake, UI: " + dialogueUIController);
    }

    public void PlaySequence(DialogueSequenceData sequence)
    {
        if (!isPlaying)
        {
            StartCoroutine(RunSequence(sequence));
        }
    }

    private IEnumerator RunSequence(DialogueSequenceData sequence)
    {
        isPlaying = true;

        foreach(DialogueLine line in sequence.lines)
        {
            dialogueUIController.ShowLine(line.text);
            yield return new WaitForSeconds(line.displayDuration);
        }

        dialogueUIController.Hide();
        isPlaying = false;
    }
}
