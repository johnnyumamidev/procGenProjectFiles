using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;

public class Elevator : MonoBehaviour, IEventListener
{
    bool elevatorActive = false;
    public float speed = 2;
    [SerializeField] GameEvent leverPulled;
    [SerializeField] UnityEvent response;
    private void OnEnable()
    {
        leverPulled.RegisterListener(this);
    }
    private void OnDisable()
    {
        leverPulled.UnregisterListener(this);
    }
    public void OnEventRaised(GameEvent gameEvent)
    {
        response?.Invoke();
    }

    private void Update()
    {
        if(elevatorActive)
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
    public void ActivateElevator()
    {
        elevatorActive = true;
    }
}
