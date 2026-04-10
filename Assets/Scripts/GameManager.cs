using UnityEngine;
using System.Collections;
using System.IO.Enumeration;

public enum GameState
{
    Crashed,
    HasScanner,
    FirstScanDone,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; } = GameState.Crashed;

    [SerializeField] private DialogueSequenceData crashDialogue;

    void Awake() => Instance = this;

    void Start()
    {
        StartCoroutine(PlayInitialDialogue());
    }

    private IEnumerator PlayInitialDialogue()
    {
        yield return new WaitForSeconds(10f);
        DialogueManager.Instance.PlaySequence(crashDialogue);
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;
    }
}