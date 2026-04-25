
using UnityEngine;

public class SolarPanel : MonoBehaviour
{
    public Transform sunDirection;
    public float alignmentThreshold = 0.95f;

    public bool isAligned = false;

    public GameObject beamEffect;

    void Update()
    {
        if (isAligned) return;

        float dot = Vector3.Dot(transform.forward, sunDirection.forward);
        
        if(dot >= alignmentThreshold)
        {
            isAligned = true;
            ActivatePanel();
        }
    }

    void ActivatePanel()
    {
        Debug.Log("Panel aligned");

        if (beamEffect != null)
        {
            beamEffect.SetActive(true);
        }

        SolarManager.Instance.PanelActivated();
    }
}
