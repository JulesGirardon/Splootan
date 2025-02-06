using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sprinklage : MonoBehaviour
{
    private PaintOnGeneratedTexture paintScript;  // Référence au script de peinture
    public float fadeInterval = 0.5f;  // Temps entre chaque diminution d'alpha
    public float fadeAmount = 0.05f;   // Quantité d'alpha retirée à chaque intervalle

    public InputActionProperty triggerAction;

    void Start()
    {
        if (triggerAction.action != null)
        {
            triggerAction.action.Enable();
            Debug.Log("Action activée: " + triggerAction.action.name);
        }


        // Désactive le script au début
        paintScript = GetComponent<PaintOnGeneratedTexture>();
        enabled = false;
    }

    public void ActivateMalus()
    {
        // Active le script lorsque le malus est déclenché
        enabled = true;
        InvokeRepeating(nameof(FadePaint), 0f, fadeInterval);
    }

    void FadePaint()
    {
        // Diminue l'alpha de toute la texture jusqu'à disparition complète
        if (paintScript != null)
        {
            Texture2D texture = paintScript.GetTexture();
            Color[] pixels = texture.GetPixels();

            bool hasPaint = false;

            for (int i = 0; i < pixels.Length; i++)
            {
                if (pixels[i].a > 0)
                {
                    pixels[i].a = Mathf.Max(0, pixels[i].a - fadeAmount);
                    hasPaint = true;
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();

            // Si toute la peinture a disparu, on arrête le processus
            if (!hasPaint)
            {
                CancelInvoke(nameof(FadePaint));
                enabled = false;
            }
        }
    }

    private void Update()
    {
        if (triggerAction.action.IsPressed())
        {
            Debug.Log("Activation siuuuuch");
            ActivateMalus();
            
        }
    }
}
