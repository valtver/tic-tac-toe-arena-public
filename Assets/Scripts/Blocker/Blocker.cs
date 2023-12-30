using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : _baseCompositionModule
{
    [Header("Behavior")]
    [SerializeField]
    private baseEvent eventAppLoadGameSession;
    [SerializeField]
    private baseEvent eventBlockerOff;
    [SerializeField]
    private baseEvent eventBlockerOn;

    void OnEnable() {

    }

     
    private void Start()
    {

    }
        
    public void ShowIntro() {
        AnimationComponent.Play("blocker-show", true);
    }

    public void Show() {
        AnimationComponent.Play("blocker-show", true).OnComplete(BlockerOnEventRaise);
    }

    // public void EndIntroLoop() {
    //     AnimationComponent.For("intro-loop").OnComplete(HideIntro);
    // }

    public void Hide() {
        AnimationComponent.Play("blocker-hide", true).OnComplete(BlockerOffEventRaise);
    }

    public void BlockerOnEventRaise() {
        // AnimationComponent.Play("intro-loop", true);
        eventAppLoadGameSession.Raise();
        eventBlockerOn.Raise();
        Debug.Log("Blocker shown");
    }

    public void BlockerOffEventRaise() {
        eventBlockerOff.Raise();
        Debug.Log("Blocker hidden");
    }

    void OnDisable() {

    }

}
