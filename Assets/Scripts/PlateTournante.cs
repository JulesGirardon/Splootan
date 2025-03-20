using UnityEngine;

public class StatueRotator : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 10f; // Vitesse de rotation en degr�s par seconde

    private bool isRotating = true;

    void Update()
    {
        if (isRotating)
        {
            transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }

    // M�thode pour activer/d�sactiver la rotation
    public void ToggleRotation()
    {
        isRotating = !isRotating;
    }

    // M�thode pour modifier la vitesse de rotation dynamiquement
    public void SetRotationSpeed(float newSpeed)
    {
        rotationSpeed = newSpeed;
    }
}
