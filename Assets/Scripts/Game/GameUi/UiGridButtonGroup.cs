using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class UiGridButtonGroup : _baseUiButtonGroup
{
     public override void Setup() {
        base.Setup();
        for(int i = 0; i < transform.childCount; i++) {
            UiGridSizeButton btn = transform.GetChild(i).gameObject.GetComponent<UiGridSizeButton>();
            if(btn.GridSize == Data.Instance.Game.GridSize.RuntimeValue) {
                btn.State = _baseUiButton.ButtonState.Selected;
            }
            else {
                btn.State = _baseUiButton.ButtonState.Default;
            }
            
        }
    }
}
