using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class UiVersusButtonGroup : _baseUiButtonGroup
{
    public override void Setup() {
        base.Setup();
        for(int i = 0; i < transform.childCount; i++) {
            UiVersusButton btn = transform.GetChild(i).gameObject.GetComponent<UiVersusButton>();
            if(btn) {
                if(btn.Versus == Data.Instance.Game.PlayerVersus.RuntimeValue) {
                    btn.State = _baseUiButton.ButtonState.Selected;
                }
                else {
                    btn.State = _baseUiButton.ButtonState.Default;
                }
            }
        }
    }
}
