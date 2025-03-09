using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
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

    public List<Vector3> tabPosition;

    private Vector3 initialWorldLocation;

    private Vector3 activePosition;
    private int activePositionIndex;

    public bool isDebug = false;

    void Start()
    {
        initialWorldLocation = gameObject.transform.position;

        hingeMotor = hingeJoints.motor;
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);

        if (tabPosition == null)
        {
            tabPosition = new List<Vector3>();
            tabPosition.Add(new Vector3(0f, 0f, 0f));
        }

        activePosition = tabPosition[0];
        activePositionIndex = 0;

        if(!isDebug)
        {
            MoveLeverToPosition(activePosition);
        }
    }

    void Update()
    {
        if (isGrabbed)
        {
            if (isDebug)
            {
                Debug.Log($"Current eulerAngles: {hingeJoints.transform.eulerAngles}");
            } 
            else
            {
                SnapLeverToPosition();
            }
        }

        
    }

    private void SnapLeverToPosition()
    {
        hingeMotor.targetVelocity = 0;
        hingeJoints.motor = hingeMotor;

        Vector3 currentAngle = hingeJoints.transform.eulerAngles;

        float minDistance = float.MaxValue;

        foreach (Vector3 vec in tabPosition)
        {
            float distance = Vector3.Distance(currentAngle, vec);
            if (distance < minDistance)
            {
                minDistance = distance;
                activePosition = vec;
                activePositionIndex = tabPosition.IndexOf(vec);
            }
        }

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
