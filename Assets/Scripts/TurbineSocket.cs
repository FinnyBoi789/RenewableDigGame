using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TurbineSocket : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] private ScannerController scannerController;
    [SerializeField] private Scanner scanner;
    private Transform turbineMesh;
    private Rigidbody turbineRigidbody;

    Vector3 rotateTurbine;


    void Awake()
    {
        XRSocketInteractor socket = GetComponent<XRSocketInteractor>();
        socket.selectEntered.AddListener(OnTurbineAttached);
        socket.selectExited.AddListener(OnTurbineRemoved);
    }

    void Update()
    {
                
        if (turbineMesh != null && scannerController.CurrentMode == ScannerMode.Mine && scanner.triggerAction.action.IsPressed())
        {
            turbineMesh.Rotate(Vector3.up, 250f * Time.deltaTime);
        }
    }

    void OnTurbineAttached(SelectEnterEventArgs args)
    {
        scannerController.hasTurbine = true;
        args.interactableObject.transform.GetComponent<Collider>().enabled = false;
        turbineRigidbody = args.interactableObject.transform.GetComponent<Rigidbody>();
        turbineMesh = args.interactableObject.transform.Find("Mesh");
        Debug.Log("turbineMesh assigned: " + turbineMesh);

        GameManager.Instance.SetState(GameState.shipDug);
    }

    void OnTurbineRemoved(SelectExitEventArgs args)
    {
        scannerController.hasTurbine = false;
        Debug.Log("Turbine Removed");
        args.interactableObject.transform.GetComponent<Collider>().enabled = true;
        turbineRigidbody = null;
    }
}
