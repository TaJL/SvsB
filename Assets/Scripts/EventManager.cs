using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    private Dictionary<string, UnityEvent> events;
    private static EventManager eventManager;

    private static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType<EventManager>();
                if (!eventManager)
                {
                    Debug.Log("No EventManager found");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    private void Init()
    {
        if (events == null)
            events = new Dictionary<string, UnityEvent>();
    }

    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent _event = null;

        if (instance.events.TryGetValue(eventName, out _event))
        {
            _event.AddListener(listener);
        }
        else
        {
            _event = new UnityEvent();
            _event.AddListener(listener);
            instance.events.Add(eventName, _event);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        UnityEvent _event = null;

        if (eventManager == null)
        {
            return;
        }

        if (instance.events.TryGetValue(eventName, out _event))
        {
            _event.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent _event = null;

        if (instance.events.TryGetValue(eventName, out _event))
        {
            _event.Invoke();
        }
    }

    private void OnDestroy()
    {
        if (eventManager == null)
        {
            return;
        }

        foreach (var _event in instance.events.Values)
        {
            _event.RemoveAllListeners();
        }
    }

}
