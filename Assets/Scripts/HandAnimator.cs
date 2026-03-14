
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
    [SerializeField] private InputActionReference selectActionReference;
    [SerializeField] private InputActionReference activateActionReference;
    [SerializeField] private Animator handAnimator;

    private static readonly int activateAnim = Animator.StringToHash("activate");
    private static readonly int selectAnim = Animator.StringToHash("select");

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
        float selectVal = selectActionReference.action?.ReadValue<float>() ?? 0f;
        float activateVal = activateActionReference.action?.ReadValue<float>() ?? 0f;
        Debug.Log($"Select: {selectVal:F2} | Activate: {activateVal:F2}");

        handAnimator.SetFloat(activateAnim, activateActionReference.action.ReadValue<float>());
        handAnimator.SetFloat(selectAnim, selectActionReference.action.ReadValue<float>());
    }
}
