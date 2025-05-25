using UnityEngine;
using UnityEngine.XR; // N'oubliez pas d'inclure ce namespace

public class CameraShakeOnShoot : MonoBehaviour
{
    [Header("Références")]
    private ClickScript clickScript;
    public Transform cameraTransform;

    [Header("Paramètres du Tremblement")]
    public float shakeDuration = 0.1f;
    public float shakeIntensity = 0.01f;

    [Header("Paramètres Haptique")]
    public float hapticAmplitude = 0.5f;
    public float hapticDuration = 0.1f;  

    private Vector3 originalCameraPosition;
    private bool isShaking = false;
    private float shakeTimer = 0f;

    void Start()
    {
        clickScript = GetComponent<ClickScript>();
        if (cameraTransform == null)
        {
            Debug.LogError("CameraTransform n'est pas assigné dans CameraShakeOnShoot.");
            return;
        }

        originalCameraPosition = cameraTransform.localPosition;
    }

    void Update()
    {
        if (Global.haptic)
        {
            if (clickScript != null && clickScript.getIsTriggerPressed())
            {
                StartShake();
            }

            if (isShaking)
            {
                if (shakeTimer > 0)
                {
                    cameraTransform.localPosition = originalCameraPosition + Random.insideUnitSphere * shakeIntensity;
                    shakeTimer -= Time.deltaTime;
                }
                else
                {
                    cameraTransform.localPosition = originalCameraPosition;
                    isShaking = false;
                }
            }
        }
    }

    public void StartShake()
    {
        if (!isShaking)
        {
            isShaking = true;
            shakeTimer = shakeDuration;
            TriggerHapticPulse(); // Ajoute la vibration sur la manette droite
        }
    }

    private void TriggerHapticPulse()
    {
        // Récupère l'appareil du contrôleur droit
        InputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        if (rightHand.isValid)
        {
            // Vérifie si l'appareil supporte les impulsions haptiques
            HapticCapabilities capabilities;
            if (rightHand.TryGetHapticCapabilities(out capabilities) && capabilities.supportsImpulse)
            {
                uint channel = 0;
                rightHand.SendHapticImpulse(channel, hapticAmplitude, hapticDuration);
            }
        }
        else
        {
            Debug.LogWarning("Contrôleur droit non valide pour les vibrations.");
        }
    }
}
