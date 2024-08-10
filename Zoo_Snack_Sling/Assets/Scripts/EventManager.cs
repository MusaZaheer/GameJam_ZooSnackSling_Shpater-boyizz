using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static event Action OnSomeEvent;

    public static void TriggerSomeEvent()
    {
        OnSomeEvent?.Invoke();
    }

    public static void ClearAllEvents()
    {
        OnSomeEvent = null; // Unsubscribe all listeners
    }
}
