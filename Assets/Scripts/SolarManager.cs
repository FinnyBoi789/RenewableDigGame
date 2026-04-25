using UnityEngine;

public class SolarManager : MonoBehaviour
{
    public static SolarManager Instance { get; private set; }

    public int totalPanels = 3;
    private int activatedPanels = 0;

    void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void PanelActivated()
    {
        activatedPanels++;

        if (activatedPanels >= totalPanels)
        {
            Debug.Log("All panels aligned!");
            GameManager.Instance.SetState(GameState.spaceshipFiredUp);
        }
    }
}