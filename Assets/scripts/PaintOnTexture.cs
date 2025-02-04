using System;
using UnityEngine;

public class PaintOnGeneratedTexture : MonoBehaviour
{
    public int textureWidth = 256;
    public int textureHeight = 256;
    public float brushSize = 20f;
    public float alphaIncrease;
    public Texture2D brushTexture; // Texture utilisée pour définir la forme du pinceau

    private Texture2D editableTexture;
    private Material material;

    void Start()
    {
        // Initialisation de la texture éditable et assignation du matériau de l'objet
        material = GetComponent<Renderer>().material;
        editableTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        Color[] pixels = new Color[textureWidth * textureHeight];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = new Color(0, 0, 0, 0);
        }
        editableTexture.SetPixels(pixels);
        editableTexture.Apply();
        material.mainTexture = editableTexture;
    }

    public void PaintAtUV(Vector2 uv)
    {
        // Calcul de la position centrale sur la texture en fonction des coordonnées UV
        int x = (int)(uv.x * editableTexture.width);
        int y = (int)(uv.y * editableTexture.height);

        // Si une texture de pinceau personnalisée est assignée, on l'utilise pour définir la forme du pinceau
        if (brushTexture != null)
        {
            Color[] brushPixels = brushTexture.GetPixels();
            int brushWidth = brushTexture.width;
            int brushHeight = brushTexture.height;
            int offsetX = brushWidth / 2;
            int offsetY = brushHeight / 2;

            // On parcourt la texture du pinceau et on applique ses pixels sur la texture éditable en tenant compte du brushSize
            for (int i = 0; i < brushWidth; i++)
            {
                for (int j = 0; j < brushHeight; j++)
                {
                    Color brushPixel = brushPixels[i + j * brushWidth];
                    if (brushPixel.a > 0) // On n'applique que les pixels non transparents du pinceau
                    {
                        int targetX = Mathf.Clamp(x + Mathf.RoundToInt((i - offsetX) * (brushSize / brushWidth)), 0, editableTexture.width - 1);
                        int targetY = Mathf.Clamp(y + Mathf.RoundToInt((j - offsetY) * (brushSize / brushHeight)), 0, editableTexture.height - 1);
                        Color targetColor = editableTexture.GetPixel(targetX, targetY);

                        targetColor.a = Mathf.Clamp01(targetColor.a + alphaIncrease * brushPixel.a);
                        targetColor.r = 0f;
                        targetColor.g = 214f / 255f;
                        targetColor.b = 255f / 255f;

                        editableTexture.SetPixel(targetX, targetY, targetColor);
                    }
                }
            }
        }
        else
        {
            // Comportement par défaut : utilisation d'un pinceau carré
            for (int i = -Mathf.FloorToInt(brushSize / 2); i < Mathf.CeilToInt(brushSize / 2); i++)
            {
                for (int j = -Mathf.FloorToInt(brushSize / 2); j < Mathf.CeilToInt(brushSize / 2); j++)
                {
                    int px = Mathf.Clamp(x + i, 0, editableTexture.width - 1);
                    int py = Mathf.Clamp(y + j, 0, editableTexture.height - 1);

                    Color pixelColor = editableTexture.GetPixel(px, py);
                    pixelColor.a = Mathf.Clamp01(pixelColor.a + alphaIncrease);
                    pixelColor.r = 0f;
                    pixelColor.g = 214f / 255f;
                    pixelColor.b = 255f / 255f;

                    editableTexture.SetPixel(px, py, pixelColor);
                }
            }
        }

        // Application des modifications sur la texture
        editableTexture.Apply();
    }
}
