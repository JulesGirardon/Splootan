using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Sprinklage : MonoBehaviour
{
    private PaintOnGeneratedTexture paintScript;  // R�f�rence au script de peinture
    public float fadeInterval = 0.5f;  // Temps entre chaque diminution d'alpha
    public float fadeAmount = 0.05f;   // Quantit� d'alpha retir�e � chaque intervalle

    public InputActionProperty triggerAction;

    void Start()
    {
        if (triggerAction.action != null)
        {
            triggerAction.action.Enable();
            Debug.Log("Action activ�e: " + triggerAction.action.name);
        }


        // D�sactive le script au d�but
        paintScript = GetComponent<PaintOnGeneratedTexture>();
        enabled = false;
    }

    public void ActivateMalus()
    {
        // Active le script lorsque le malus est d�clench�
        enabled = true;
        InvokeRepeating(nameof(FadePaint), 0f, fadeInterval);
    }

    void FadePaint()
    {
        // Diminue l'alpha de toute la texture jusqu'� disparition compl�te
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

            // Si toute la peinture a disparu, on arr�te le processus
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
