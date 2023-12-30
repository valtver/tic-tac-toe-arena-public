using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiAudioButton : _baseUiButton
{
    [Header("Behavior")]
    public baseEvent eventUiDataAudioChange;
    public AppAudioVar AppAudioVar;
    // Start is called before the first frame update
    public override void Setup()
    {
        if((int)AppAudioVar.RuntimeValue > 0) {
            State = ButtonState.Selected;
        }
        else if((int)AppAudioVar.RuntimeValue < 0) {
            State = ButtonState.Default;
        }
        else {
            State = ButtonState.Inactive;
        }
    }

    public override void Click() {
        // base.Click();
        if(eventUiButtonClick)
            eventUiButtonClick.Raise();
        AnimationComponent.Play("button-click", true);
        AppAudioVar.RuntimeValue = (AudioVar)(-(int)AppAudioVar.RuntimeValue);
        eventUiDataAudioChange.Raise();
        Debug.Log("Button audiovar = " + AppAudioVar.RuntimeValue);
        Debug.Log("Data Audiovars = " + Data.Instance.App.Sound.RuntimeValue + " and " + Data.Instance.App.Music.RuntimeValue);
        Setup();
    }
}
