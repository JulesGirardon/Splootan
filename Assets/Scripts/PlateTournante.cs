using UnityEngine;

public class StatueRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10f; // Vitesse de rotation en degrés par seconde
    private bool isRotating = true;

    void Update()
    {
        if (isRotating)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }

    // Méthode pour activer/désactiver la rotation
    public void ToggleRotation()
    {
        isRotating = !isRotating;
    }

    // Méthode pour modifier la vitesse de rotation dynamiquement
    public void SetRotationSpeed(float newSpeed)
    {
        rotationSpeed = newSpeed;
    }
}
