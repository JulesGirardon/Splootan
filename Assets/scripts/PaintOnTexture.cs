using System;
using UnityEngine;

public class PaintOnGeneratedTexture : MonoBehaviour
{
    public int textureWidth = 256;
    public int textureHeight = 256;
    public float brushSize = 20f;
    public float alphaIncrease = 0.1f;

    private Texture2D editableTexture;
    private Material material;

    void Start()
    {
        // Initialise une texture vide et l'assigne au matériau
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
        // Applique un effet de tache de peinture avec un dégradé radial
        int centerX = (int)(uv.x * editableTexture.width);
        int centerY = (int)(uv.y * editableTexture.height);
        int radius = Mathf.FloorToInt(brushSize / 2);

        for (int i = -radius; i <= radius; i++)
        {
            for (int j = -radius; j <= radius; j++)
            {
                float distance = Mathf.Sqrt(i * i + j * j) / radius;
                if (distance <= 1f)
                {
                    float intensity = Mathf.Pow(1f - distance, 2);

                    int targetX = Mathf.Clamp(centerX + i, 0, editableTexture.width - 1);
                    int targetY = Mathf.Clamp(centerY + j, 0, editableTexture.height - 1);

                    Color targetColor = editableTexture.GetPixel(targetX, targetY);
                    targetColor.a = Mathf.Clamp01(targetColor.a + alphaIncrease * intensity);
                    targetColor.r = 0f;
                    targetColor.g = 214f / 255f;
                    targetColor.b = 255f / 255f;

                    editableTexture.SetPixel(targetX, targetY, targetColor);
                }
            }
        }

        // Met à jour la texture affichée
        editableTexture.Apply();
    }

    public Texture2D GetTexture()
    {
        return editableTexture;
    }
}
