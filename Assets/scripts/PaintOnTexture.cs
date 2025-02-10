using System;
using UnityEngine;

public class PaintOnGeneratedTexture : MonoBehaviour
{
    public int textureWidth = 256;
    public int textureHeight = 256;
    public float brushSize = 20f;
    public float alphaIncrease = 0.1f;

    private Texture2D maskTexture;
    private Material material;

    void Start()
    {
        // Récupère le matériau et crée une texture masque vide (alpha = 0 partout)
        material = GetComponent<Renderer>().material;
        maskTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        Color[] pixels = new Color[textureWidth * textureHeight];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = new Color(0, 0, 255, 0);
        }
        maskTexture.SetPixels(pixels);
        maskTexture.Apply();

        // Affecte la texture masque à la propriété "Mask" du shader
        material.SetTexture("_Mask", maskTexture);
    }

    public void PaintAtUV(Vector2 uv)
    {
        // Calcule la position centrale sur le masque et applique un dégradé radial
        int centerX = (int)(uv.x * maskTexture.width);
        int centerY = (int)(uv.y * maskTexture.height);
        int radius = Mathf.FloorToInt(brushSize / 2);

        for (int i = -radius; i <= radius; i++)
        {
            for (int j = -radius; j <= radius; j++)
            {
                float distance = Mathf.Sqrt(i * i + j * j) / radius;
                if (distance <= 1f)
                {
                    float intensity = Mathf.Pow(1f - distance, 2);
                    int targetX = Mathf.Clamp(centerX + i, 0, maskTexture.width - 1);
                    int targetY = Mathf.Clamp(centerY + j, 0, maskTexture.height - 1);

                    Color targetColor = maskTexture.GetPixel(targetX, targetY);
                    targetColor.a = Mathf.Clamp01(targetColor.a + alphaIncrease * intensity);
                    maskTexture.SetPixel(targetX, targetY, targetColor);
                }
            }
        }
        maskTexture.Apply();

        // Met à jour la propriété "Mask" du shader avec la texture modifiée
        material.SetTexture("Mask", maskTexture);
    }

    public Texture2D GetMaskTexture()
    {
        return maskTexture;
    }
}
