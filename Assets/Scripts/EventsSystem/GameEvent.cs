using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Game Event", menuName ="Event")]
public class GameEvent : ScriptableObject
{
    private readonly List<IEventListener> eventListeners = new List<IEventListener>();

    public void Raise()
    {
        for(int i = eventListeners.Count-1; i >= 0; i--)
        {
            eventListeners[i].OnEventRaised(this);
        }
    }

    public void RegisterListener(IEventListener listener)
    {
        if (!eventListeners.Contains(listener))
        {
            eventListeners.Add(listener);
        }
    }

    public void UnregisterListener(IEventListener listener)
    {
        if(eventListeners.Contains(listener)) { eventListeners.Remove(listener); }
    }
}
