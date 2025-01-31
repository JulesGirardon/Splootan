using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ClickScript : MonoBehaviour
{
    public Camera mainCamera;
    public InputActionProperty triggerAction; // � assigner dans l'Inspector
    public GameObject projectilePrefab; // Pr�fab de projectile � assigner
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
        if (triggerAction.action.WasPressedThisFrame()) // V�rifie si la g�chette a �t� press�e
        {
            Debug.Log("Clic d�tect� !"); // Log quand le clic est effectu�

            // Instancier un projectile � la position de la cam�ra
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
                Debug.Log($"Touch� : {hit.collider.gameObject.name} � {hit.point}");

              

                // R�cup�rer le script PaintOnGeneratedTexture attach� � l'objet touch�
                PaintOnGeneratedTexture paintScript = hit.collider.gameObject.GetComponent<PaintOnGeneratedTexture>();

                if (paintScript != null)
                {
                    // Calculer les coordonn�es UV de l'impact
                    Renderer renderer = hit.collider.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        Vector2 uv = hit.textureCoord;
                        Debug.Log($"Coordonn�es UV de l'impact : {uv}");

                        // Peindre � cet endroit
                        paintScript.PaintAtUV(uv);
                    }
                }
            }
        }
    }
}
