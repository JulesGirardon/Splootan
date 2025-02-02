using System;
using UnityEngine;

public class PaintOnGeneratedTexture : MonoBehaviour
{
    public int textureWidth = 256;  
    public int textureHeight = 256; 
    public float brushSize = 20f;   
    public float alphaIncrease; 

    private Texture2D editableTexture;
    private Material material;

    void Start()
    {
        material = GetComponent<Renderer>().sharedMaterial;

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
        int x = (int)(uv.x * editableTexture.width);
        int y = (int)(uv.y * editableTexture.height);

        for (int i = -Mathf.FloorToInt(brushSize / 2); i < Mathf.CeilToInt(brushSize / 2); i++)
        {
            for (int j = -Mathf.FloorToInt(brushSize / 2); j < Mathf.CeilToInt(brushSize / 2); j++)
            {
                int px = Mathf.Clamp(x + i, 0, editableTexture.width - 1);
                int py = Mathf.Clamp(y + j, 0, editableTexture.height - 1);

                Color pixelColor = editableTexture.GetPixel(px, py);

                pixelColor.a = Mathf.Clamp01(pixelColor.a + alphaIncrease);
                pixelColor.r = 1f;

                editableTexture.SetPixel(px, py, pixelColor);
            }
        }

        editableTexture.Apply();
    }

}
