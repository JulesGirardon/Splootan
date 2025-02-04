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

                if (impact != null)
                {
                    impact.SetActive(false);
                }
            }

            Debug.DrawRay(gunTip.position, gunTip.forward * rayLength, Color.red);

            RaycastHit hit;
            if (Physics.Raycast(gunTip.position, gunTip.forward, out hit, rayLength))
            {
                Debug.Log($"Touché : {hit.collider.gameObject.name} à {hit.point}");

                

                PaintOnGeneratedTexture paintScript = hit.collider.gameObject.GetComponent<PaintOnGeneratedTexture>();
                if (paintScript != null)
                {
                    if (impact != null)
                    {
                        impact.SetActive(true);
                    }

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
        else
        {
            if (laser != null)
            {
                laser.SetActive(false);
            }
        }
    }
}
