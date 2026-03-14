
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HandAnimator : MonoBehaviour
{

    [SerializeField] private NearFarInteractor nearFarInteractor;
    [SerializeField] private SkinnedMeshRenderer handMesh;
    [SerializeField] private inputActionReference selectActionReference;
    [SerializeField] private inputActionReference activateActionReference;

    private void Awake()
    {
        nearFarInteractor.selectEntered.AddListener(OnGrab);
        nearFarInteractor.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("Selected");
        handMesh.enabled = false;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        Debug.Log("Deselected");
        handMesh.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
