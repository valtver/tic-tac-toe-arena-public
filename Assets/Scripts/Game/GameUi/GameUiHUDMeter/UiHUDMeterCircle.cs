using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiHUDMeterCircle : _baseUiMeter
{
    public SymbolVar SymbolId;

    public override IEnumerator Init()
    {
        yield return StartCoroutine(base.Init());

    }

    public override void Setup()
    {
        base.Setup();
        gameObject.SetActive(true);
        SetPlayer();
    }

    public void SetPlayer() {
        if(SymbolId == Data.Instance.Game.ActivePlayer.symbol) {
            ShowSymbol();
        }
        else {
            HideSymbol();
        }
    }

    private void ShowSymbol() {
        AnimationComponent.SpineUiComponent.Play("o-show").OnComplete(()=>{
            AnimationComponent.SpineUiComponent.Loop("o-loop");
        });
    }

    private void HideSymbol() {
        AnimationComponent.SpineUiComponent.Play("o-hide");
    }
}
