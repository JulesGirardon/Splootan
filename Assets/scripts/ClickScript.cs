using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ClickScript : MonoBehaviour
{
    public Transform gunTip;
    public InputActionProperty triggerAction;
    public float rayLength = 100f;

    public GameObject laser;

    private GameObject impact;
    private GameObject beam;

    void Start()
    {
        if (triggerAction.action != null)
        {
            triggerAction.action.Enable();
        }

        if (laser != null)
        {
            impact = laser.transform.Find("impact")?.gameObject;
            beam = laser.transform.Find("beam")?.gameObject;
            laser.SetActive(false);
        }
    }

    void Update()
    {
        if (triggerAction.action.IsPressed())
        {
            if (laser != null)
            {
                laser.SetActive(true);
            }

            Debug.DrawRay(gunTip.position, gunTip.forward * rayLength, Color.red);

            RaycastHit hit;
            // Si le raycast touche un objet...
            if (Physics.Raycast(gunTip.position, gunTip.forward, out hit, rayLength))
            {
                Debug.Log($"Touché : {hit.collider.gameObject.name} à {hit.point}");

                // On vérifie si l'objet touché possède le script de peinture
                PaintOnGeneratedTexture paintScript = hit.collider.gameObject.GetComponent<PaintOnGeneratedTexture>();
                if (paintScript != null)
                {
                    if (impact != null)
                    {
                        impact.SetActive(true);
                        impact.transform.position = hit.point;
                    }

                    Renderer renderer = hit.collider.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        Vector2 uv = hit.textureCoord;
                        Debug.Log($"Coordonnées UV de l'impact : {uv}");
                        paintScript.PaintAtUV(uv);
                    }
                }
                // Si l'objet touché ne possède pas le script, on désactive l'impact
                else
                {
                    if (impact != null)
                    {
                        impact.SetActive(false);
                    }
                }
            }
            // Si le raycast ne touche rien, on désactive également l'impact
            else
            {
                if (impact != null)
                {
                    impact.SetActive(false);
                }
            }
        }
        else
        {
            if (laser != null)
            {
                laser.SetActive(false);
                if (impact != null)
                {
                    impact.SetActive(false);
                }
            }
        }
    }
}
