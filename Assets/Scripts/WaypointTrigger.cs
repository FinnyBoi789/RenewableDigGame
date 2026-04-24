using System.ComponentModel;
using UnityEngine;

public class WaypointTrigger : MonoBehaviour
{
    [SerializeField] private WaypointIndicator waypointIndicator;
    [SerializeField] private Canvas waypointCanvas;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioSource waypointAudio;
    [SerializeField] private AudioClip waypoint;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            waypointIndicator.gameObject.SetActive(false);
            waypointCanvas.gameObject.SetActive(false);
            waypointAudio.PlayOneShot(waypoint);
            GameManager.Instance.SetState(GameState.waypointTriggered);
            // other things go here later
        }
    }
}
