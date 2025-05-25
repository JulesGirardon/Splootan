using System.Collections;
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
    private float snapSpeed = 5f;

    public List<Vector3> tabPosition;

    private Vector3 initialWorldLocation;

    private Vector3 activePosition;
    private int activePositionIndex;

    public bool isDebug = false;

    [SerializeField] private Function activeFunction;

    private Coroutine launchCoroutine;

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

        if (!isDebug)
        {
            SetSettings(activeFunction);
            LaunchFunction();
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

        LaunchFunction();
    }

    private void SetSettings(Function activeFunction)
    {
        switch (activeFunction) {
            case Function.EnableHaptic:
                if (Global.haptic)
                {
                    activePositionIndex = 0;
                }
                else
                {
                    activePositionIndex = 1;
                }
                break;
            case Function.EnableMusic:
                if (Global.musicVolume == 0)
                {
                    activePositionIndex = 0;
                }
                else if (Global.musicVolume == 25)
                {
                    activePositionIndex = 1;
                }
                else if (Global.musicVolume == 50)
                {
                    activePositionIndex = 2;
                }
                else if (Global.musicVolume == 75)
                {
                    activePositionIndex = 3;
                }
                else if (Global.musicVolume == 100)
                {
                    activePositionIndex = 4;
                }
                break;
            case Function.EnableSFX:
                if (Global.sfxVolume == 0)
                {
                    activePositionIndex = 0;
                }
                else if (Global.sfxVolume == 25)
                {
                    activePositionIndex = 1;
                }
                else if (Global.sfxVolume == 50)
                {
                    activePositionIndex = 2;
                }
                else if (Global.sfxVolume == 75)
                {
                    activePositionIndex = 3;
                }
                else if (Global.sfxVolume == 100)
                {
                    activePositionIndex = 4;
                }
                break;
            case Function.ChangeQuality:
                activePositionIndex = QualitySettings.GetQualityLevel();
                break;
        }
        Debug.Log(activePositionIndex);

        hingeMotor.targetVelocity = 0;
        hingeJoints.motor = hingeMotor;
        hingeJoints.transform.eulerAngles = tabPosition[activePositionIndex];
        gameObject.transform.position = initialWorldLocation;
    }

    public enum Function
    {
        EnableHaptic,
        EnableMusic,
        EnableSFX,
        LaunchGame,
        ChangeQuality
    }

    void LaunchFunction()
    {
        switch (activeFunction)
        {
            case Function.EnableHaptic:
                EnableHaptic();
                break;
            case Function.EnableMusic:
                EnableSound(ref Global.musicVolume);
                break;
            case Function.EnableSFX:
                EnableSound(ref Global.sfxVolume);
                break;
            case Function.LaunchGame:
                HandleLaunchGame();
                break;
            case Function.ChangeQuality:
                ChangeQuality();
                break;
        }
    }

    private void EnableHaptic()
    {
        if (activePositionIndex == 0)
        {
            Global.haptic = true;
        }
        else
        {
            Global.haptic = false;
        }

    }

    private void EnableSound(ref int sound)
    {
        if (activePositionIndex == 0)
        {
            sound = 0;
        }
        else if (activePositionIndex == 1)
        {
            sound = 25;
        }
        else if (activePositionIndex == 2 || activePositionIndex == 5)
        {
            sound = 50;
        }
        else if (activePositionIndex == 3)
        {
            sound = 75;
        }
        else if (activePositionIndex == 4)
        {
            sound = 100;
        }
    }

    private void HandleLaunchGame()
    {
        if (activePositionIndex == 1)
        {
            if (launchCoroutine == null)
            {
                launchCoroutine = StartCoroutine(LaunchGameWithDelay());
            }
        }
        else
        {
            if (launchCoroutine != null)
            {
                StopCoroutine(launchCoroutine);
                launchCoroutine = null;
                Debug.Log("Launch cancelled");
            }
        }
    }

    private IEnumerator LaunchGameWithDelay()
    {
        yield return new WaitForSeconds(5f);
        if (activePositionIndex == 1)
        {
            Debug.Log("Launching Game");
            UnityEngine.SceneManagement.SceneManager.LoadScene("BasicScene");
        }
    }

    private void ChangeQuality()
    {
        QualitySettings.SetQualityLevel(activePositionIndex, true);
    }
}
