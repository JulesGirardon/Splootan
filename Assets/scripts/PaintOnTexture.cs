using System.Collections;
using UnityEngine;

public class PaintOnGeneratedTexture : MonoBehaviour
{
    public int textureWidth = 256;
    public int textureHeight = 256;
    public float brushSize = 28f;
    public float alphaIncrease = 0.03f;

    private Texture2D maskTexture;
    private Material material;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        maskTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        
        Color[] pixels = new Color[textureWidth * textureHeight];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = new Color(0, 0, 0, 0);
        }
        maskTexture.SetPixels(pixels);
        maskTexture.Apply();

        material.SetTexture("_Mask", maskTexture);
    }

    public void PaintAtUV(Vector2 uv)
    {
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
        material.SetTexture("_Mask", maskTexture);
    }
    public void EraseAtUV(Vector2 uv)
    {
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
                    targetColor.a = Mathf.Clamp01(targetColor.a - alphaIncrease * intensity);
                    maskTexture.SetPixel(targetX, targetY, targetColor);
                }
            }
        }
        maskTexture.Apply();
        material.SetTexture("_Mask", maskTexture);
    }

    public void CalculatePaintArea()
    {
        int paintedPixels = 0;
        Color[] pixels = maskTexture.GetPixels();

        foreach (Color pixel in pixels)
        {
            if (pixel.a > 0f)
            {
                paintedPixels++;
            }
        }

        float paintedPercentage = (float)paintedPixels / pixels.Length * 100f;
        Debug.Log("Surface peinte : " + paintedPercentage + "%");
    }
}
