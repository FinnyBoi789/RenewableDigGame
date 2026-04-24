using UnityEngine;
using System.Collections;
using System.IO.Enumeration;
using System.Runtime.Serialization;

public enum GameState
{
    Crashed,
    HasScanner,
    FirstScanDone,
    ScannedEnvironment,
    waypointTriggered,
    pickedUpTurbine,
    shipDug,
    checkedSpaceship,
    spaceshipFiredUp
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameState CurrentState { get; private set; } = GameState.Crashed;

    public int ObjectsScanned { get; private set; } = 0;
    public int TotalObjects = 3; // set in Inspector

    [SerializeField] private WaypointIndicator waypointIndicator;
    [SerializeField] private GameObject waypoint;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip dopamineAudioClip;

    public void LogScannedObject()
    {
        ObjectsScanned++;
        Debug.Log("Objects scanned: " + ObjectsScanned);
    }

    [SerializeField] private DialogueSequenceData crashDialogue;
    [SerializeField] private DialogueSequenceData scanner;
    [SerializeField] private DialogueSequenceData firstScanDone;
    [SerializeField] private DialogueSequenceData scannedEnvironment;
    [SerializeField] private DialogueSequenceData waypointTriggered;
    [SerializeField] private DialogueSequenceData pickedUpTurbine;
    [SerializeField] private DialogueSequenceData shipDug;
    [SerializeField] private DialogueSequenceData checkedSpaceship;
    [SerializeField] private DialogueSequenceData spaceshipFiredUp;
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
        if (CurrentState == newState) return;

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
                audioSource.PlayOneShot(dopamineAudioClip);
                progressMenu.AddEntry("✓ Scanned the environment");
                waypoint.SetActive(true);
                waypointIndicator.SetTarget(waypoint.transform);
                DialogueManager.Instance.PlaySequence(scannedEnvironment);
                break;
            case GameState.waypointTriggered:
                DialogueManager.Instance.PlaySequence(waypointTriggered);
                break;
            case GameState.pickedUpTurbine:
                DialogueManager.Instance.PlaySequence(pickedUpTurbine);
                break;
            case GameState.shipDug:
                DialogueManager.Instance.PlaySequence(shipDug);
                break;
            case GameState.checkedSpaceship:
                DialogueManager.Instance.PlaySequence(checkedSpaceship);
                break;
            case GameState.spaceshipFiredUp:
                DialogueManager.Instance.PlaySequence(spaceshipFiredUp);
                break;
        }
    }
}