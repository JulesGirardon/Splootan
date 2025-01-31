using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ClickScript : MonoBehaviour
{
    public Camera mainCamera;
    public InputActionProperty triggerAction; // À assigner dans l'Inspector
    public GameObject projectilePrefab; // Préfab de projectile à assigner
    public float projectileSpeed = 10f; // Vitesse du projectile

    void Start()
    {
        if (triggerAction.action != null)
        {
            triggerAction.action.Enable();
        }
    }

    [System.Obsolete]
    void Update()
    {
        if (triggerAction.action.WasPressedThisFrame()) // Vérifie si la gâchette a été pressée
        {
            Debug.Log("Clic détecté !"); // Log quand le clic est effectué

            // Instancier un projectile à la position de la caméra
            if (projectilePrefab != null)
            {
                GameObject projectile = Instantiate(projectilePrefab, mainCamera.transform.position, Quaternion.identity);
                Rigidbody rb = projectile.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.velocity = mainCamera.transform.forward * projectileSpeed; // Lancer vers l'avant
                }
            }

            RaycastHit hit;
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit))
            {
                Debug.Log($"Touché : {hit.collider.gameObject.name} à {hit.point}");

              

                // Récupérer le script PaintOnGeneratedTexture attaché à l'objet touché
                PaintOnGeneratedTexture paintScript = hit.collider.gameObject.GetComponent<PaintOnGeneratedTexture>();

                if (paintScript != null)
                {
                    // Calculer les coordonnées UV de l'impact
                    Renderer renderer = hit.collider.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        Vector2 uv = hit.textureCoord;
                        Debug.Log($"Coordonnées UV de l'impact : {uv}");

                        // Peindre à cet endroit
                        paintScript.PaintAtUV(uv);
                    }
                }
            }
        }
    }
}
