using UnityEngine;

public class WaypointTrigger : MonoBehaviour
{
    [SerializeField] private WaypointIndicator waypointIndicator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            waypointIndicator.gameObject.SetActive(false);
            // other things go here later
        }
    }
}
