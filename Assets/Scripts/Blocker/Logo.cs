using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo : _baseCompositionModule
{
    [Header("Behavior")]
    public baseEvent eventAppLoadGameSession;
    
    public void Show() {
        AnimationComponent.Play("logo-show", true).OnComplete(()=>{
            eventAppLoadGameSession.Raise(this.gameObject);
            AnimationComponent.Play("logo-loop", true);
            });
    }

    public void EndIntroLoop() {
        AnimationComponent.Play("logo-hide", true).OnComplete(DisableMe);
    }

    public void Hide() {
        AnimationComponent.Play("logo-hide", true).OnComplete(DisableMe);
    }

    public void DisableMe() {
        this.gameObject.SetActive(false);
    }

}
