using System;
using System.Collections;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public IEnumerator TimerCR(float duration, Action onComplete)
    {
        while (Game.IsPaused) yield return null;
        yield return new WaitForSeconds(duration);
        onComplete?.Invoke();
    }
}
