using System.ComponentModel;
using UnityEngine;

public class WaypointTrigger : MonoBehaviour
{
    [SerializeField] private WaypointIndicator waypointIndicator;
    [SerializeField] private Canvas waypointCanvas;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            waypointIndicator.gameObject.SetActive(false);
            waypointCanvas.gameObject.SetActive(false);
            // other things go here later
        }
    }
}
