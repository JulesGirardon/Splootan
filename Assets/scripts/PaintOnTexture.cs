using System;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class PaintOnGeneratedTexture : MonoBehaviour
{
    public int textureWidth = 256;  // Largeur de la texture
    public int textureHeight = 256; // Hauteur de la texture
    public float brushSize = 20f;   // Taille de l'impact
    public float alphaIncrease; // Valeur d'alpha � augmenter (1 = totalement opaque)
    public float impactInterval; // Intervalle entre chaque impact (en secondes)

    private Texture2D editableTexture;
    private float timeSinceLastImpact = 0f;
    private Material material;

    void Start()
    {
        // Obtenir le mat�riau du Renderer de l'objet (utiliser sharedMaterial pour affecter le mat�riau dans l'inspecteur aussi)
        material = GetComponent<Renderer>().sharedMaterial; // Utilisation de sharedMaterial

        // Cr�er une texture vide (transparente)
        editableTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);

        // Initialiser toute la texture en transparent (alpha = 0)
        Color[] pixels = new Color[textureWidth * textureHeight];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = new Color(0, 0, 0, 0); // Transparent
        }
        editableTexture.SetPixels(pixels);
        editableTexture.Apply();

        // Appliquer cette texture au mat�riau de l'objet
        material.mainTexture = editableTexture;
    }

    public void PaintAtUV(Vector2 uv)
    {
        int x = (int)(uv.x * editableTexture.width);
        int y = (int)(uv.y * editableTexture.height);

        for (int i = -Mathf.FloorToInt(brushSize / 2); i < Mathf.CeilToInt(brushSize / 2); i++)
        {
            for (int j = -Mathf.FloorToInt(brushSize / 2); j < Mathf.CeilToInt(brushSize / 2); j++)
            {
                int px = Mathf.Clamp(x + i, 0, editableTexture.width - 1);
                int py = Mathf.Clamp(y + j, 0, editableTexture.height - 1);

                // R�cup�rer la couleur actuelle du pixel
                Color pixelColor = editableTexture.GetPixel(px, py);

                // Augmenter l'alpha sans d�passer 1
                pixelColor.a = Mathf.Clamp01(pixelColor.a + alphaIncrease);
                pixelColor.r = 255;

                // Appliquer la nouvelle couleur
                editableTexture.SetPixel(px, py, pixelColor);
            }
        }

        editableTexture.Apply();
    }

    void Update()
    {
        // Calculer le temps �coul�
        timeSinceLastImpact += Time.deltaTime;

        // Si le temps depuis le dernier impact est sup�rieur � l'intervalle d�fini
        if (timeSinceLastImpact >= impactInterval)
        {
            // Appliquer un impact al�atoire
            Vector2 randomUV = new Vector2(UnityEngine.Random.value, UnityEngine.Random.value);
            PaintAtUV(randomUV);

            // R�initialiser le timer
            timeSinceLastImpact = 0f;
        }
    }
}
