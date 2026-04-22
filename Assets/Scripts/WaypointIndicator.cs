using UnityEngine;

public class WaypointIndicator : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Transform arrow;
    [SerializeField] private Transform playerCamera;
    void Update()
    {
        if (target == null || !target.gameObject.activeInHierarchy)
            return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            float angle = Vector3.SignedAngle(playerCamera.forward, direction, Vector3.up);
            arrow.localRotation = Quaternion.Euler(90, angle, 0);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        arrow.gameObject.SetActive(true);
    }
}