using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ButtonFollowVisual : MonoBehaviour
{
    public Transform visualTarget;
    private Transform pokeAttachTransform;
    public float resetSpeed = 5f;
    public float followAngleThreshold = 45f;
    private XRBaseInteractable interactable;
    public Vector3 localAxis;
    private Vector3 initialLocalPos;
    private bool isFollowing = false;
    private Vector3 offset;
    private bool freeze = false;

    bool hasPressed = false;
    [SerializeField] float pressDepth = 0.02f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialLocalPos = visualTarget.localPosition;

        interactable = GetComponent<XRBaseInteractable>();
        interactable.hoverEntered.AddListener(Follow);
        interactable.hoverExited.AddListener(Reset);
        interactable.selectEntered.AddListener(Freeze);
    }

    public void Follow(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            XRPokeInteractor interactor = (XRPokeInteractor)hover.interactorObject;
            pokeAttachTransform = interactor.attachTransform;
            offset = visualTarget.position - pokeAttachTransform.position;

            isFollowing = true;
            freeze = false;
        }
    }

    public void Reset(BaseInteractionEventArgs hover)
    {
        if(hover.interactorObject is XRPokeInteractor)
        {
            isFollowing = false;
            freeze = false;
        }
    }

    public void Freeze(BaseInteractionEventArgs hover)
    {
        if (hover.interactorObject is XRPokeInteractor)
        {
            freeze = true;
            StartCoroutine(UnfreezeAfterDelay());
        }
    }

    IEnumerator UnfreezeAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        freeze = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (freeze)
            return;

        // --- Movement ---
        if (isFollowing)
        {
            Vector3 localTargetPosition = visualTarget.InverseTransformPoint(pokeAttachTransform.position + offset);
            Vector3 constrainedLocalTargetPosition = Vector3.Project(localTargetPosition, localAxis);
            visualTarget.position = visualTarget.TransformPoint(constrainedLocalTargetPosition);
        }
        else
        {
            visualTarget.localPosition = Vector3.Lerp(
                visualTarget.localPosition,
                initialLocalPos,
                Time.deltaTime * resetSpeed
            );
        }

        // --- Press logic ---
        float pressPoint = initialLocalPos.y - pressDepth;
        float releasePoint = initialLocalPos.y - (pressDepth * 0.5f);

        if (!hasPressed && visualTarget.localPosition.y < pressPoint)
        {
            hasPressed = true;
            Debug.Log("BUTTON PRESSED");
            GameManager.Instance.SetState(GameState.checkedSpaceship);
        }

        // --- Release logic ---
        if (hasPressed && visualTarget.localPosition.y > releasePoint)
        {
            hasPressed = false;
        }
    }
}
