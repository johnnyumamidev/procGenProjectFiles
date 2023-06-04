using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    [SerializeField] private Dictionary<string, UnityAction> _events = new Dictionary<string, UnityAction>();

    private static EventManager _eventManager;
    public static EventManager instance
    {
        get
        {
            if (!_eventManager)
            {
                _eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!_eventManager)
                    Debug.LogError("There needs to be one active EventManager script on a GameObject in your scene.");
                else
                    _eventManager.Init();
            }
            return _eventManager;
        }
    }

    void Init()
    {
        if (_events == null)
            _events = new Dictionary<string, UnityAction>();
    }

    public void AddListener(string eventName, UnityAction listener)
    {
        if (_events.ContainsKey(eventName))
        {
            Debug.Log("event manager already contains " + eventName);
            return;
        }
        Debug.Log("adding " + eventName + " to event manager");
        instance._events.Add(eventName, listener);
    }

    public void RemoveListener(string eventName, UnityAction listener)
    {
        instance._events.Remove(eventName, out listener);
    }

    public void TriggerEvent(string eventName)
    {
        if(instance._events.ContainsKey(eventName))
        {
            Debug.Log(eventName + " triggered");
            instance._events[eventName].Invoke();
        }
    }
}
