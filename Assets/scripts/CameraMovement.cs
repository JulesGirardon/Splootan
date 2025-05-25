using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement4Inputs : MonoBehaviour
{
    [Tooltip("Input pour aller à gauche sur l'axe X")]
    public InputActionProperty leftAction;
    [Tooltip("Input pour aller à droite sur l'axe X")]
    public InputActionProperty rightAction;
    [Tooltip("Input pour aller en haut sur l'axe Y")]
    public InputActionProperty upAction;
    [Tooltip("Input pour aller en bas sur l'axe Y")]
    public InputActionProperty downAction;

    [Tooltip("Vitesse de déplacement de la caméra")]
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
        // Lire les valeurs d'entrée pour chaque direction.
        // Par exemple, si l'action renvoie 1 lorsque le bouton est pressé et 0 sinon.
        float leftValue = leftAction.action.ReadValue<float>();
        float rightValue = rightAction.action.ReadValue<float>();
        float upValue = upAction.action.ReadValue<float>();
        float downValue = downAction.action.ReadValue<float>();

        // Calculer le mouvement net sur l'axe X : (droite - gauche)
        float xInput = rightValue - leftValue;
        // Calculer le mouvement net sur l'axe Y : (haut - bas)
        float yInput = upValue - downValue;

        // Créer le vecteur de déplacement (ici, on ne déplace que sur X et Y)
        Vector3 movement = new Vector3(xInput, yInput, 0f);

        // Appliquer le déplacement en fonction du temps
        transform.position += movement * movementSpeed * Time.deltaTime;
    }
}
