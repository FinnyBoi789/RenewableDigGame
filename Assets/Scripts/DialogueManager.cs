using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    
    private bool isPlaying = false;

    void Awake()
    {
        Instance = this;
    }

    public void PlaySequence(DialogueSequenceData sequence)
    {
        if (!isPlaying)
        {
            StartCoroutine(RunSequence(sequence));
        }
    }

    public void StopDialogue()
    {
        StopAllCoroutines();
        audioSource.Stop();
        isPlaying = false;
    }

    private IEnumerator RunSequence(DialogueSequenceData sequence)
    {
        isPlaying = true;

        foreach(DialogueLine line in sequence.lines)
        {
            audioSource.clip = line.clip;
            audioSource.Play();
            yield return new WaitForSeconds(line.clip.length + line.delayAfter);
        }
        isPlaying = false;
    }
}
