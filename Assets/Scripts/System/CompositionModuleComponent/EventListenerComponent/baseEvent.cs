using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Events/Event", order = 1)]
public class baseEvent : ScriptableObject {
    
    private readonly List<EventListenerComponent> eventListeners =
            new List<EventListenerComponent>();

    public void Raise(GameObject gameObject = null)
    {
        Debug.Log(this.name + " RISE!");
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaised(gameObject);
    }

    public void RegisterListener(EventListenerComponent listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(EventListenerComponent listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    } 
}
