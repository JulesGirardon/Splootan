using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ClickScript : MonoBehaviour
{
    public Camera mainCamera;
    public InputActionProperty triggerAction;
    public GameObject projectilePrefab; 
    public float projectileSpeed = 10f; 

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
        if (triggerAction.action.WasPressedThisFrame())
        {

            if (projectilePrefab != null)
            {
                GameObject projectile = Instantiate(projectilePrefab, mainCamera.transform.position, Quaternion.identity);
                Rigidbody rb = projectile.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.velocity = mainCamera.transform.forward * projectileSpeed;
                }
            }

            RaycastHit hit;
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit))
            {
                Debug.Log($"Touché : {hit.collider.gameObject.name} à {hit.point}");



                PaintOnGeneratedTexture paintScript = hit.collider.gameObject.GetComponent<PaintOnGeneratedTexture>();

                if (paintScript != null)
                {
                    Renderer renderer = hit.collider.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        Vector2 uv = hit.textureCoord;
                        Debug.Log($"Coordonnées UV de l'impact : {uv}");

                        paintScript.PaintAtUV(uv);
                    }
                }
            }
        }
    }
}