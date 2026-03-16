using UnityEngine;

public class GrabAnimator : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer handMeshR;
    [SerializeField] private SkinnedMeshRenderer handMeshL;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        handMeshR.enabled = false;
        handMeshL.enabled = false;
    }

    public void selectEnter()
    {
        handMeshR.enabled = true;
        handMeshL.enabled = true;
    }

    public void selectExit()
    {
        handMeshR.enabled = false;
        handMeshL.enabled = false;
    }
}
