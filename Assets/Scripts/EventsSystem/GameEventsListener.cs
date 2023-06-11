using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventsListener : MonoBehaviour, IEventListener
{
    [SerializeField] GameEvent gameEvent;
    [SerializeField] UnityEvent response;

    private void OnEnable()
    {
        if(gameEvent != null) gameEvent.RegisterListener(this); 
    }
    private void OnDisable()
    {
        gameEvent?.UnregisterListener(this);
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        response?.Invoke();
    }
}
