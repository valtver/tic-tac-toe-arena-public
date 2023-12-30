using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiVersusButton : _baseUiButton
{
    [Header("Behavior")]
    public Versus Versus;
    public baseEvent eventDataVersusChange;
    // Start is called before the first frame update
    public override void Click() {
        base.Click();
        AnimationComponent.Play("button-click", true);
        State = ButtonState.Selected;
        eventDataVersusChange.Raise(this.gameObject);
    }
}
