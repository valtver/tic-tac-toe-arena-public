using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadIndicator : _baseCompositionModule
{
    [Header("Behavior")]
    public baseEvent eventAppLoadGameSession;
    
    public void Show() {
        AnimationComponent.Play("blocker-load-show", true);
        AnimationComponent.Play("blocker-load-loop", 1, true);
    }

    public void Hide() {
        AnimationComponent.Play("blocker-load-hide", true).OnComplete(()=>{
            AnimationComponent.Play("default", 1, true);
        });
    }

    public void DisableMe() {
        this.gameObject.SetActive(false);
    }

}
