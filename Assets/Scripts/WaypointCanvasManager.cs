
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.UI;
using System.Runtime.Serialization;
public class WaypointCanvasManager : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI distanceText;
    [SerializeField] private GameObject waypoint;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform waypointCanvas;

    public void Update()
    {
        float distance = Vector3.Distance(player.transform.position, waypoint.transform.position);
        distanceText.text = $"Energy Source Found: {distance:F1}m";

        waypointCanvas.localRotation = Quaternion.LookRotation(waypointCanvas.position - player.transform.position);
    }
}
