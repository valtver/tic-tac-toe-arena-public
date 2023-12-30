using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HecticPlayLogo : _baseCompositionModule
{
    [Header("Behavior")]
    public baseEvent eventBlockerShowIntro;
    
    public void Show() {
        AnimationComponent.Play("hectic-play-intro", true).OnComplete(()=>{
            eventBlockerShowIntro.Raise(this.gameObject);
            DisableMe();
            });
    }

    public void DisableMe() {
        this.gameObject.SetActive(false);
    }
}
