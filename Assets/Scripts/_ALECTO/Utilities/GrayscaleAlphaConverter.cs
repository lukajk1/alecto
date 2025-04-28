using UnityEngine;
using System.IO;

public class GrayscaleAlphaConverter : MonoBehaviour
{
    public Texture2D inputTexture;

    void Start()
    {
        if (inputTexture == null)
            return;

        Texture2D outputTexture = new Texture2D(inputTexture.width, inputTexture.height, TextureFormat.RGBA32, false);
        Color[] pixels = inputTexture.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            float grayscale = pixels[i].grayscale;
            float alpha = 1f - grayscale; // black -> 1, white -> 0
            pixels[i] = new Color(pixels[i].r, pixels[i].g, pixels[i].b, alpha);
        }

        outputTexture.SetPixels(pixels);
        outputTexture.Apply();

        byte[] bytes = outputTexture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/ConvertedImage.png", bytes);
    }
}
