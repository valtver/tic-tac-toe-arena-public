using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSymbolButton : _baseUiButton
{
    [Header("Behavior")]
    public SymbolVar Symbol;
    public baseEvent eventDataSymbolChange;
    // Start is called before the first frame update
    public override void Click() {
        base.Click();
        AnimationComponent.Play("button-click", true);
        State = ButtonState.Selected;
        eventDataSymbolChange.Raise(this.gameObject);
    }
}
