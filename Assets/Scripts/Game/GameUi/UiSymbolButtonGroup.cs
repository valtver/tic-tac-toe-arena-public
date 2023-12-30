using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class UiSymbolButtonGroup : _baseUiButtonGroup
{
    public override void Setup() {
        base.Setup();
        for(int i = 0; i < transform.childCount; i++) {
            UiSymbolButton btn = transform.GetChild(i).gameObject.GetComponent<UiSymbolButton>();
            if(btn) {
                if(btn.Symbol == Data.Instance.Game.Symbol.RuntimeValue) {
                    btn.State = _baseUiButton.ButtonState.Selected;
                }
                else {
                    btn.State = _baseUiButton.ButtonState.Default;
                }
            }
        }
    }
}
