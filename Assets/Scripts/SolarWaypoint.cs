using UnityEngine;

public class SolarWaypoint : MonoBehaviour
{
    private bool hasActivated = false;

    void Update()
    {
        if (!hasActivated && GameManager.Instance.CurrentState == GameState.checkedSpaceship)
        {
            Spawn();
            hasActivated = true;
        }
    }

    void Spawn()
    {
        gameObject.SetActive(true);
    }
}