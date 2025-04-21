using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private TextMeshProUGUI fpsCounter;
    private float[] fpsBuffer = new float[20];
    private int fpsBufferIndex = 0;
    private int bufferCount = 0;

    private void Awake()
    {
        fpsCounter = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        float currentFPS = 1f / Time.unscaledDeltaTime;
        AddToBuffer(currentFPS);
        fpsCounter.text = GetAverageFPS().ToString("F1");
    }

    void AddToBuffer(float newFPS)
    {
        fpsBuffer[fpsBufferIndex] = newFPS;
        fpsBufferIndex = (fpsBufferIndex + 1) % fpsBuffer.Length;
        if (bufferCount < fpsBuffer.Length) bufferCount++;
    }

    float GetAverageFPS()
    {
        float total = 0f;
        for (int i = 0; i < bufferCount; i++)
            total += fpsBuffer[i];
        return total / bufferCount;
    }
}
