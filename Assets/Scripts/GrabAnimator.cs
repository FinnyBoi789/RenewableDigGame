using UnityEngine;

public class GrabAnimator : MonoBehaviour
{
    [SerializeField] private MeshRenderer handMesh;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        handMesh.enabled = false;
    }

    public void selectEnter()
    {
        handMesh.enabled = true;
    }

    public void selectExit()
    {
        handMesh.enabled = false;
    }
}
