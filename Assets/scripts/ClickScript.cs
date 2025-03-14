using Unity.Properties;
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
    private Animator animator;

    private bool isTriggerPressed;

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

        animator = GetComponent<Animator>();
        animator.enabled = false;
    }
    public bool getIsTriggerPressed()
    {
        return isTriggerPressed;
    }
    void Update()
    {
        isTriggerPressed = triggerAction.action.IsPressed();

        if (animator != null)
        {
            animator.enabled = isTriggerPressed;
        }

        if (isTriggerPressed)
        {
            if (laser != null)
            {
                laser.SetActive(true);
            }

            Debug.DrawRay(gunTip.position, gunTip.forward * rayLength, Color.red);

            RaycastHit hit;
            if (Physics.Raycast(gunTip.position, gunTip.forward, out hit, rayLength))
            {
                PaintOnGeneratedTexture paintScript = hit.collider.gameObject.GetComponent<PaintOnGeneratedTexture>();
                if (paintScript != null)
                {
                    if (impact != null)
                    {
                        impact.SetActive(true);
                        impact.transform.position = hit.point;
                    }

                    Vector2 uv = hit.textureCoord;
                 
                    paintScript.PaintAtUV(uv);
                }
                else if (impact != null)
                {
                    impact.SetActive(false);
                }
            }
            else if (impact != null)
            {
                impact.SetActive(false);
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
