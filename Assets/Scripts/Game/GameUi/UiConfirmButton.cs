using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiConfirmButton : _baseUiButton
{
    [Header("Behavior")]
    public baseEvent eventUiConfirmChanges;

    public override void Setup() {
        base.Setup();
        State = ButtonState.Selected;
    }
    // Start is called before the first frame update
    public override void Click() {
        AnimationComponent.Play("button-click", true);
        if(eventUiConfirmChanges) eventUiConfirmChanges.Raise(this.gameObject);
        if(eventUiButtonClick)
            eventUiButtonClick.Raise();
    }
}
