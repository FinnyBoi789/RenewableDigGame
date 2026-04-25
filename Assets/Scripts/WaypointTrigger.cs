using System.ComponentModel;
using UnityEngine;

public class WaypointTrigger : MonoBehaviour
{
    [SerializeField] private GameObject waypointIndicator;
    [SerializeField] private Canvas waypointCanvas;
    [SerializeField] private AudioSource waypointAudio;
    [SerializeField] private AudioClip waypoint;
    public bool turbineWaypoint = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {   
            if (waypointAudio != null && waypointAudio.gameObject.activeInHierarchy)
            {
                waypointAudio.PlayOneShot(waypoint);
            }
            
            gameObject.SetActive(false);

            if (turbineWaypoint)
            {
                GameManager.Instance.SetState(GameState.waypointTriggered);
            }
            else
            {
                GameManager.Instance.AdvanceSolarWaypoint();
            }


        }
    }
}
