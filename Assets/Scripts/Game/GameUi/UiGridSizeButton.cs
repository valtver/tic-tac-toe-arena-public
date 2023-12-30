using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiGridSizeButton : _baseUiButton
{
    [Header("Behavior")]
    public GridSize GridSize;
    public baseEvent eventUiDataGridSizeChange;
    // Start is called before the first frame update
    public override void Click() {
        base.Click();
        AnimationComponent.Play("button-click", true);
        State = ButtonState.Selected;
        eventUiDataGridSizeChange.Raise(this.gameObject);
    }

}
