using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class RoomScript : MonoBehaviour
{
    // Reference to the TimerManager component
    private TimerManager timerManager;

    // Input action for shooting; assign via the Inspector.
    public InputActionProperty triggerAction;

    // Maximum distance allowed to activate the timer
    public float activationDistance = 5f;

    // Maximum raycast distance when checking for a hit
    public float raycastDistance = 10f;

    public float timeTimer = 300.0f;

    public List<GameObject> alarms;

    public GameObject statue;

    private void Awake()
    {
        if (triggerAction != null)
        {
            // Enable the trigger action
            triggerAction.action.Enable();
        }
        else
        {
            Debug.LogError("Trigger Action is not assigned!");
        }
    }

    private void Update()
    {
        // Check if the trigger action was pressed this frame.
        // Using WasPressedThisFrame() ensures we only process once per press.
        if (triggerAction != null && triggerAction.action.WasPressedThisFrame())
        {
            TryActivateTimer();
        }
    }

    private void TryActivateTimer()
    {
        // Get the main camera.
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("Main Camera not found!");
            return;
        }

        // Create a ray from the center of the screen.
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        // Perform a raycast
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
        {
            // Check if the hit object is this cube.
            if (hit.collider.gameObject == gameObject)
            {
                // Check if the player is close enough.
                // Here, we assume the player's position is that of the camera.
                float distance = Vector3.Distance(transform.position, cam.transform.position);
                if (distance <= activationDistance)
                {
                    StartTime();
                }
                else
                {
                    Debug.LogWarning("Player is too far from the cube to activate the timer.");
                }
            }
        }
    }

    private void StartTime()
    {
        // Retrieve the TimerManager instance.
        timerManager = TimerManager.Instance;

        if (timerManager != null)
        {
            // Check if the timer is not already running (assuming getTimer() returns 0.0f when idle)
            if (timerManager.GetTimer() == 0.0f)
            {
                timerManager.StartTimer(timeTimer,statue,alarms);
                Debug.Log("Timer started for 10 seconds!");
            }
            else
            {
                Debug.LogError("Can't start a new timer while one is already running!");
            }
        }
        else
        {
            Debug.LogError("TimerManager not found!");
        }
    }
}
