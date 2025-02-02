using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement4Inputs : MonoBehaviour
{
    [Tooltip("Input pour aller � gauche sur l'axe X")]
    public InputActionProperty leftAction;
    [Tooltip("Input pour aller � droite sur l'axe X")]
    public InputActionProperty rightAction;
    [Tooltip("Input pour aller en haut sur l'axe Y")]
    public InputActionProperty upAction;
    [Tooltip("Input pour aller en bas sur l'axe Y")]
    public InputActionProperty downAction;

    [Tooltip("Vitesse de d�placement de la cam�ra")]
    public float movementSpeed = 2f;

    void OnEnable()
    {
        leftAction.action.Enable();
        rightAction.action.Enable();
        upAction.action.Enable();
        downAction.action.Enable();
    }

    void Update()
    {
        // Lire les valeurs d'entr�e pour chaque direction.
        // Par exemple, si l'action renvoie 1 lorsque le bouton est press� et 0 sinon.
        float leftValue = leftAction.action.ReadValue<float>();
        float rightValue = rightAction.action.ReadValue<float>();
        float upValue = upAction.action.ReadValue<float>();
        float downValue = downAction.action.ReadValue<float>();

        // Calculer le mouvement net sur l'axe X : (droite - gauche)
        float xInput = rightValue - leftValue;
        // Calculer le mouvement net sur l'axe Y : (haut - bas)
        float yInput = upValue - downValue;

        // Cr�er le vecteur de d�placement (ici, on ne d�place que sur X et Y)
        Vector3 movement = new Vector3(xInput, yInput, 0f);

        // Appliquer le d�placement en fonction du temps
        transform.position += movement * movementSpeed * Time.deltaTime;
    }
}
