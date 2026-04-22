using UnityEngine;
using System.Collections;
using System.IO.Enumeration;

public enum GameState
{
    Crashed,
    HasScanner,
    FirstScanDone,
    ScannedEnvironment,
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; } = GameState.Crashed;

    public int ObjectsScanned { get; private set; } = 0;
    public int TotalObjects = 3; // set in Inspector

    [SerializeField] private WaypointIndicator waypointIndicator;
    [SerializeField] private GameObject waypoint;

    public void LogScannedObject()
    {
        ObjectsScanned++;
        Debug.Log("Objects scanned: " + ObjectsScanned);
    }

    [SerializeField] private DialogueSequenceData crashDialogue;
    [SerializeField] private DialogueSequenceData scanner;
    [SerializeField] private DialogueSequenceData firstScanDone;
    [SerializeField] private DialogueSequenceData scannedEnvironment;
    [SerializeField] private ProgressScreen progressMenu;


    void Awake() => Instance = this;

    void Start()
    {
        StartCoroutine(PlayInitialDialogue());
    }

    private IEnumerator PlayInitialDialogue()
    {
        yield return new WaitForSeconds(5f);
        DialogueManager.Instance.PlaySequence(crashDialogue);

    }


    //Also handles updating the progress screen
    public void SetState(GameState newState)
    {
        CurrentState = newState;

        DialogueManager.Instance.StopDialogue();

        switch (newState)
    {
        case GameState.HasScanner:
            progressMenu.AddEntry("✓ Found the scanner");
            DialogueManager.Instance.PlaySequence(scanner);
            break;
        case GameState.FirstScanDone:
            progressMenu.AddEntry("✓ Scanned first object");
            DialogueManager.Instance.PlaySequence(firstScanDone);
            break;
        case GameState.ScannedEnvironment:
            progressMenu.AddEntry("✓ Scanned the environment");
            waypoint.SetActive(true);
            waypointIndicator.SetTarget(waypoint.transform);
            break;
    }
    }
}