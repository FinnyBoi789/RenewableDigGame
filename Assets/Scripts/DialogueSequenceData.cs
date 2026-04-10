using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueSequence", menuName = "Dialogue/Dialogue Sequence")]
public class DialogueSequenceData : ScriptableObject
{
    public List<DialogueLine> lines;
}
