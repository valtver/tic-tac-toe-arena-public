using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class EventListenerComponent : _baseCompositionModuleComponent
 {
     [Tooltip("Event to register with.")]
     public baseEvent Event;

     [Tooltip("Response to invoke when Event is raised.")]
     public baseResponse Response;

     private void OnEnable()
     {
         Event.RegisterListener(this);
        //  Debug.Log(this.gameObject + " listeners subscribed!");
     }

     private void OnDisable()
     {
         Event.UnregisterListener(this);
        //  Debug.Log(this.gameObject + " listeners unsubscribed!");
     }

     public void OnEventRaised(GameObject gameObject)
     {
         Response.Invoke(gameObject);
     }
 }
