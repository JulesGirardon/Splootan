using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LeverController : MonoBehaviour
{
    public XRGrabInteractable grabInteractable;
    public HingeJoint hingeJoints;
    private JointMotor hingeMotor;
    private bool isGrabbed = false;
    private bool isAtPosition1 = true; // Position 1 = (300, 270, 180), Position 2 = (300, 90, 0)
    private float snapSpeed = 5f;

    // Positions possibles
    private Vector3 position1 = new Vector3(300f, 270f, 180f); // Position 1: 300, 270, 180
    private Vector3 position2 = new Vector3(300f, 90f, 0f);   // Position 2: 300, 90, 0

    private Vector3 initialWorldLocation;

    private Vector3 activePosition;

    void Start()
    {
        initialWorldLocation = gameObject.transform.position;

        hingeMotor = hingeJoints.motor;
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);

        activePosition = position1;
    }

    void Update()
    {
        Debug.Log($"Current eulerAngles: {hingeJoints.transform.eulerAngles}");
        Debug.Log($"Lever position 1 = {isAtPosition1}");

        if (isGrabbed)
        {
            SnapLeverToPosition();
        }
    }

    private void SnapLeverToPosition()
    {
        hingeMotor.targetVelocity = 0;
        hingeJoints.motor = hingeMotor;

        Vector3 currentAngle = hingeJoints.transform.eulerAngles;

        // Calculer la distance entre la position actuelle et les deux positions possibles
        float distanceToPosition1 = Vector3.Distance(currentAngle, position1);
        float distanceToPosition2 = Vector3.Distance(currentAngle, position2);

        // Choisir la position la plus proche
        activePosition = distanceToPosition1 < distanceToPosition2 ? position1 : position2;
        MoveLeverToPosition(activePosition);
    }

    private void MoveLeverToPosition(Vector3 targetPosition)
    {
        hingeMotor.targetVelocity = 0;
        hingeJoints.motor = hingeMotor;

        // Appliquer la position cible directement
        hingeJoints.transform.eulerAngles = targetPosition;
        gameObject.transform.position = initialWorldLocation;

        // Log pour le débogage
        Debug.Log($"Moving to closest position: {targetPosition}");
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        MoveLeverToPosition(activePosition);
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isGrabbed = false;
        MoveLeverToPosition(activePosition);
    }
}
