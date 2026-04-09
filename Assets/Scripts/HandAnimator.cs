
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
    [SerializeField] private GameObject handArmature;
    [SerializeField] private InputActionReference selectActionReference;
    [SerializeField] private InputActionReference activateActionReference;
    [SerializeField] private Animator handAnimator;
    [SerializeField] private float ActionDelay = 0.3f;

    private static readonly int activateAnim = Animator.StringToHash("activate");
    private static readonly int selectAnim = Animator.StringToHash("select");
    private static readonly int grabAnim = Animator.StringToHash("grab");

    private bool emptyHand;

    private void Awake()
    {
        nearFarInteractor.selectEntered.AddListener(OnGrab);
        nearFarInteractor.selectExited.AddListener(OnRelease);
        emptyHand = true;
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("Selected");
        handAnimator.SetBool(grabAnim, true);
        emptyHand = false;
        //handArmature.SetActive(false);
        //handMesh.enabled = false;
        StartCoroutine(DelayedGrab());
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        handAnimator.SetBool(grabAnim, false);
        StartCoroutine(DelayedRelease());
    }

    private IEnumerator DelayedGrab()
    {
        yield return new WaitForSeconds(ActionDelay);
    }

    private IEnumerator DelayedRelease()
    {
        yield return new WaitForSeconds(ActionDelay);
        //handMesh.enabled = true;
        handArmature.SetActive(true);
        emptyHand = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (emptyHand)
        {
            handAnimator.SetFloat(activateAnim, activateActionReference.action.ReadValue<float>());
            handAnimator.SetFloat(selectAnim, selectActionReference.action.ReadValue<float>());
        }
        
    }
}
