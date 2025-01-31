using System;
using UnityEngine;

public class PaintOnGeneratedTexture : MonoBehaviour
{
    public int textureWidth = 256;  // Largeur de la texture
    public int textureHeight = 256; // Hauteur de la texture
    public float brushSize = 20f;   // Taille de l'impact
    public float alphaIncrease; // Valeur d'alpha à augmenter (1 = totalement opaque)

    private Texture2D editableTexture;
    private Material material;

    void Start()
    {
        // Obtenir le matériau du Renderer de l'objet (utiliser sharedMaterial pour affecter le matériau dans l'inspecteur aussi)
        material = GetComponent<Renderer>().sharedMaterial; // Utilisation de sharedMaterial

        // Créer une texture vide (transparente)
        editableTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);

        // Initialiser toute la texture en transparent (alpha = 0)
        Color[] pixels = new Color[textureWidth * textureHeight];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = new Color(0, 0, 0, 0); // Transparent
        }
        editableTexture.SetPixels(pixels);
        editableTexture.Apply();

        // Appliquer cette texture au matériau de l'objet
        material.mainTexture = editableTexture;
    }

    public void PaintAtUV(Vector2 uv)
    {
        int x = (int)(uv.x * editableTexture.width);
        int y = (int)(uv.y * editableTexture.height);

        // Appliquer l'impact avec la taille de pinceau
        for (int i = -Mathf.FloorToInt(brushSize / 2); i < Mathf.CeilToInt(brushSize / 2); i++)
        {
            for (int j = -Mathf.FloorToInt(brushSize / 2); j < Mathf.CeilToInt(brushSize / 2); j++)
            {
                int px = Mathf.Clamp(x + i, 0, editableTexture.width - 1);
                int py = Mathf.Clamp(y + j, 0, editableTexture.height - 1);

                // Récupérer la couleur actuelle du pixel
                Color pixelColor = editableTexture.GetPixel(px, py);

                // Augmenter l'alpha sans dépasser 1
                pixelColor.a = Mathf.Clamp01(pixelColor.a + alphaIncrease);
                pixelColor.r = 1f; // Rendre l'impact rouge (modifie la couleur selon ce que tu veux)

                // Appliquer la nouvelle couleur
                editableTexture.SetPixel(px, py, pixelColor);
            }
        }

        editableTexture.Apply();
    }

    void Update()
    {
        // La logique d'impact aléatoire est supprimée, car on veut peindre uniquement sur demande
    }
}
