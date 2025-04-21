using System;
using UnityEngine;

public class MainEventBus
{
    public static event Action OnRunStart;
    public static void BCOnRunStart() => OnRunStart?.Invoke();

    public static event Action OnRunEnd;
    public static void BCOnRunEnd() => OnRunEnd?.Invoke();
}
