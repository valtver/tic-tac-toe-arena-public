using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiDeclineButton : _baseUiButton
{
    [Header("Behavior")]
    public baseEvent eventUiDeclineChanges;

    public override void Setup() {
        base.Setup();
        State = ButtonState.Selected;
    }
    // Start is called before the first frame update
    public override void Click() {
        AnimationComponent.Play("button-click", true);
        if(eventUiDeclineChanges) eventUiDeclineChanges.Raise(this.gameObject);
        if(eventUiButtonClick)
            eventUiButtonClick.Raise();
    }
}
