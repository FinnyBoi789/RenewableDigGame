using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueSequenceData sequence;

    private void Trigger()
    {
        DialogueManager.Instance.PlaySequence(sequence);
    }
}
