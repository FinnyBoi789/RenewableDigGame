using UnityEngine;
using System.Collections;
using System.IO.Enumeration;

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
    public int TotalObjects = 3;

    public int SolarObjectsScanned {get; private set; } = 0;
    public int SolarTotalObjects = 3;

    private bool solarPhaseActive = false;
    private bool solarUnlocked = false;

    [SerializeField] private GameObject waypoint;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip dopamineAudioClip;


    [SerializeField] private AudioSource spaceShipAudioSource;
    [SerializeField] private AudioClip spaceshipStartUpFail;
    [SerializeField] private AudioClip spaceshipStartUpSuccess;


    [SerializeField] private DialogueSequenceData crashDialogue;
    [SerializeField] private DialogueSequenceData scanner;
    [SerializeField] private DialogueSequenceData firstScanDone;
    [SerializeField] private DialogueSequenceData scannedEnvironment;
    [SerializeField] private DialogueSequenceData waypointTriggered;
    [SerializeField] private DialogueSequenceData pickedUpTurbine;
    [SerializeField] private DialogueSequenceData shipDug;
    [SerializeField] private DialogueSequenceData checkedSpaceship;
    [SerializeField] private DialogueSequenceData spaceshipFiredUp;

    [SerializeField] private CanvasGroup fadeGroup;
    [SerializeField] private float fadeDuration = 2f;

    IEnumerator FadeToBlack()
    {
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = t / fadeDuration;
            yield return null;
        }

        fadeGroup.alpha = 1f;

        yield return new WaitForSeconds(15f);

        Debug.Log("GAME COMPLETE");
        Application.Quit();
    }

    [SerializeField] private GameObject[] solarWaypoints;

    int panelsAligned = 0;
    [SerializeField] int totalPanels = 3;

    public void PanelAligned()
    {
        panelsAligned++;
        Debug.Log("Panels aligned: " + panelsAligned);

        if (panelsAligned >= totalPanels)
        {
            SetState(GameState.spaceshipFiredUp);
        }
    }

    int currentSolarIndex = 0;
    void ActivateSolarWaypoints()
    {
        currentSolarIndex = 0;

        foreach (var wp in solarWaypoints)
        {
            wp.SetActive(false);
        }

        Debug.Log("Activating solar waypoint: " + solarWaypoints[0].name);
        Debug.Log("Active before: " + solarWaypoints[0].activeSelf);

        solarWaypoints[0].SetActive(true);

        Debug.Log("Active after: " + solarWaypoints[0].activeSelf);
    }

    public void AdvanceSolarWaypoint()
    {
        solarWaypoints[currentSolarIndex].SetActive(false);

        currentSolarIndex++;

        if (currentSolarIndex < solarWaypoints.Length)
        {
            solarWaypoints[currentSolarIndex].SetActive(true);
        }
    }


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

    public void RegisterSpaceship(AudioSource source)
    {
        spaceShipAudioSource = source;
    }

    public void LogScan()
    {
        Debug.Log("LOGSCAN CALLED");

        ObjectsScanned++;
        Debug.Log("Objects scanned: " + ObjectsScanned);

        Debug.Log("solarPhaseActive: " + solarPhaseActive);
        Debug.Log("Solar count BEFORE: " + SolarObjectsScanned);

        if (ObjectsScanned >= TotalObjects && CurrentState < GameState.ScannedEnvironment)
        {
            Debug.Log("SETTING ScannedEnvironment");
            SetState(GameState.ScannedEnvironment);
        }

        if (solarPhaseActive && !solarUnlocked)
        {
            SolarObjectsScanned++;
            Debug.Log("Solar count AFTER: " + SolarObjectsScanned);

            if (SolarObjectsScanned >= SolarTotalObjects)
            {
                solarUnlocked = true;
                Debug.Log("ACTIVATING SOLAR WAYPOINTS");
                audioSource.PlayOneShot(dopamineAudioClip);
                ActivateSolarWaypoints();
            }
        }
    }

    //Also handles updating the progress screen
    public void SetState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;

        Debug.Log("STATE SET TO: " + newState);

        DialogueManager.Instance.StopDialogue();
        

        switch (newState)
        {
            case GameState.HasScanner:
                DialogueManager.Instance.PlaySequence(scanner);
                break;
            case GameState.FirstScanDone:
                DialogueManager.Instance.PlaySequence(firstScanDone);
                break;
            case GameState.ScannedEnvironment:
                audioSource.PlayOneShot(dopamineAudioClip);
                waypoint.SetActive(true);
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
                if (spaceShipAudioSource != null)
                {
                    spaceShipAudioSource.PlayOneShot(spaceshipStartUpFail);
                }
                else
                {
                    Debug.LogError("No AudioSource found on spaceship!");
                }
                DialogueManager.Instance.PlaySequence(checkedSpaceship);
                solarPhaseActive = true;
                SolarObjectsScanned = 0;
                Debug.Log("Solar Phase Started");
                break;
            case GameState.spaceshipFiredUp:
                if (spaceShipAudioSource != null)
                {
                    spaceShipAudioSource.PlayOneShot(spaceshipStartUpSuccess);
                }
                DialogueManager.Instance.PlaySequence(spaceshipFiredUp);
                StartCoroutine(FadeToBlack());
                break;
        }
    }
}