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
                    eventManager.InitEventDictionary();
                }
            }

            return eventManager;
        }
    }

    private void InitEventDictionary()
    {
        if (events == null)
            events = new Dictionary<string, UnityEvent>();
    }

    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent e = null;

        if (instance.events.TryGetValue(eventName, out e))
        {
            e.AddListener(listener);
        }
        else
        {
            e = new UnityEvent();
            e.AddListener(listener);
            instance.events.Add(eventName, e);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent e = null;

        if (instance.events.TryGetValue(eventName, out e))
        {
            e.Invoke();
        }
    }

    private void OnDestroy()
    {
        if (eventManager == null)
        {
            return;
        }

        foreach (var e in instance.events.Values)
        {
            e.RemoveAllListeners();
        }
    }

}
