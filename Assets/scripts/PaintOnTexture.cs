using System.Collections;
using UnityEngine;

public class PaintOnGeneratedTexture : MonoBehaviour
{
    public int textureWidth = 256;
    public int textureHeight = 256;
    public float brushSize = 28f;
    public float alphaIncrease = 0.03f;

    private Texture2D maskTexture;
    private Color[] pixels;
    private Material material;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        maskTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        pixels = new Color[textureWidth * textureHeight];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = new Color(0, 0, 0, 0); // Texture initiale transparente
        }
        maskTexture.SetPixels(pixels);
        maskTexture.Apply();
        material.SetTexture("_Mask", maskTexture);
    }

    public void PaintAtUV(Vector2 uv)
    {
        int centerX = (int)(uv.x * textureWidth);
        int centerY = (int)(uv.y * textureHeight);
        int radius = Mathf.FloorToInt(brushSize / 2);

        for (int i = -radius; i <= radius; i++)
        {
            for (int j = -radius; j <= radius; j++)
            {
                float distance = Mathf.Sqrt(i * i + j * j) / radius;
                if (distance <= 1f)
                {
                    int targetX = Mathf.Clamp(centerX + i, 0, textureWidth - 1);
                    int targetY = Mathf.Clamp(centerY + j, 0, textureHeight - 1);
                    int index = targetX + targetY * textureWidth;

                    float intensity = Mathf.Pow(1f - distance, 2);
                    pixels[index].a = Mathf.Clamp01(pixels[index].a + intensity * alphaIncrease);
                }
            }
        }
        maskTexture.SetPixels(pixels);
        maskTexture.Apply();
    }

    public void ApplyGravity()
    {
        for (int y = textureHeight - 2; y >= 0; y--)
        {
            for (int x = 0; x < textureWidth; x++)
            {
                int index = x + y * textureWidth;
                int belowIndex = x + (y + 1) * textureWidth;

                if (pixels[index].a > 0.9f && pixels[belowIndex].a < 0.9f)
                {
                    float dripAmount = 0.05f;
                    pixels[index].a = Mathf.Max(0.85f, pixels[index].a - dripAmount);
                    pixels[belowIndex].a = Mathf.Min(1.0f, pixels[belowIndex].a + dripAmount * 1.2f);
                }
            }
        }
        maskTexture.SetPixels(pixels);
        maskTexture.Apply();
    }

    void Update()
    {
        ApplyGravity();
    }
}
